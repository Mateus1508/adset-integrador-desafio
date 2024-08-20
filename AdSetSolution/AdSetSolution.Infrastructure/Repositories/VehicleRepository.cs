using AdSetSolution.Application.DTOs;
using AdSetSolution.Domain.Interfaces;
using AdSetSolution.Domain.Models;
using AdSetSolution.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AdSetSolution.Infrastructure.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<VehicleRepository> _logger;

        public VehicleRepository(AppDbContext context, ILogger<VehicleRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Vehicle>> GetFilteredVehicles(VehicleFilter filter)
        {
            try
            {
                var query = _context.Vehicles
                    .Include(v => v.VehicleImgs)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(filter.Marca))
                    query = query.Where(v => v.Marca.Contains(filter.Marca));

                if (!string.IsNullOrEmpty(filter.Modelo))
                    query = query.Where(v => v.Modelo.Contains(filter.Modelo));

                if (filter.AnoMin.HasValue)
                    query = query.Where(v => v.Ano >= filter.AnoMin.Value);

                if (filter.AnoMax.HasValue)
                    query = query.Where(v => v.Ano <= filter.AnoMax.Value);

                if (!string.IsNullOrEmpty(filter.Placa))
                    query = query.Where(v => v.Placa.Contains(filter.Placa));

                if (!string.IsNullOrEmpty(filter.Cor))
                    query = query.Where(v => v.Cor.Contains(filter.Cor));

                if (!string.IsNullOrEmpty(filter.Preco))
                {
                    var precoRange = filter.Preco;
                    if (precoRange == "10000-50000")
                        query = query.Where(v => v.Preco >= 10000 && v.Preco <= 50000);
                    else if (precoRange == "50000-90000")
                        query = query.Where(v => v.Preco > 50000 && v.Preco <= 90000);
                    else if (precoRange == "90000+")
                        query = query.Where(v => v.Preco > 90000);
                }

                if(!string.IsNullOrEmpty(filter.Fotos))
                {
                    if (filter.Fotos == "ComFotos")
                        query = query.Where(v => v.VehicleImgs != null && v.VehicleImgs.Any());
                    else if (filter.Fotos == "SemFotos")
                        query = query.Where(v => v.VehicleImgs == null || !v.VehicleImgs.Any());
                }

                if (!string.IsNullOrEmpty(filter.Opcionais))
                    query = query.Where(v => v.Opcionais.Contains(filter.Opcionais));

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao filtrar veículos.");
                throw;
            }
        }

        public async Task<Vehicle> GetVehicleById(int id)
        {
            try
            {
                return await _context.Vehicles
                    .Include(v => v.VehicleImgs)
                    .FirstOrDefaultAsync(v => v.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter o veículo de Id {Id}.", id);
                throw;
            }
        }

        public async Task<int> AddVehicle(Vehicle vehicle)
        {
            try
            {
                await _context.Vehicles.AddAsync(vehicle);

                await _context.SaveChangesAsync();

                return vehicle.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar veículo.");
                throw;
            }
        }

        public async Task<bool> UpdateVehicle(Vehicle vehicle)
        {
            try
            {
                _context.Vehicles.Update(vehicle);
                int result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Erro ao atualizar veículo de Id {Id}.", vehicle.Id);
                throw;
            }
        }

        public async Task<bool> DeleteVehicle(int id)
        {
            try
            {
                var vehicle = await _context.Vehicles.FindAsync(id);
                if (vehicle != null)
                {
                    _context.Vehicles.Remove(vehicle);
                    int result = await _context.SaveChangesAsync();
                    return result > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir veículo de Id {Id}.", id);
                throw;
            }
        }
    }
}
