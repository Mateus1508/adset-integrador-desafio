using AdSetSolution.Domain.Interfaces;
using AdSetSolution.Domain.Models;
using AdSetSolution.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

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
                    .Include(v => v.VehiclePackages)
                    .Include(v => v.VehicleOptional)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(filter.Brand))
                    query = query.Where(v => v.Brand.Contains(filter.Brand));

                if (!string.IsNullOrEmpty(filter.Model))
                    query = query.Where(v => v.Model.Contains(filter.Model));

                if (filter.YearMin.HasValue)
                    query = query.Where(v => v.Year >= filter.YearMin.Value);

                if (filter.YearMax.HasValue)
                    query = query.Where(v => v.Year <= filter.YearMax.Value);

                if (!string.IsNullOrEmpty(filter.LicensePlate))
                    query = query.Where(v => v.LicensePlate.Contains(filter.LicensePlate));

                if (!string.IsNullOrEmpty(filter.Color))
                    query = query.Where(v => v.Color.Contains(filter.Color));

                if (!string.IsNullOrEmpty(filter.Price))
                {
                    var priceRange = filter.Price;
                    if (priceRange == "10000-50000")
                        query = query.Where(v => v.Price >= 10000 && v.Price <= 50000);
                    else if (priceRange == "50000-90000")
                        query = query.Where(v => v.Price > 50000 && v.Price <= 90000);
                    else if (priceRange == "90000+")
                        query = query.Where(v => v.Price > 90000);
                }

                if (!string.IsNullOrEmpty(filter.Photos))
                {
                    if (filter.Photos == "ComFotos")
                        query = query.Where(v => v.VehicleImgs != null && v.VehicleImgs.Any());
                    else if (filter.Photos == "SemFotos")
                        query = query.Where(v => v.VehicleImgs == null || !v.VehicleImgs.Any());
                }

                if (filter.VehicleOptionalIds != null && filter.VehicleOptionalIds.Any())
                {
                    query = query.Where(v => filter.VehicleOptionalIds.All(id =>
                        v.VehicleOptional.Any(vo => vo.OptionalId == id)));
                }

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
                    .Include(v => v.VehicleOptional)
                    .ThenInclude(vo => vo.Optional)
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
            catch (Exception ex)
            {
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
