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

        public async Task<IEnumerable<Vehicle>> GetAllVehicles()
        {
            try
            {
                return await _context.Vehicles.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os veículos.");
                throw;
            }
        }

        public async Task<Vehicle> GetVehicleById(int id)
        {
            try
            {
                return await _context.Vehicles.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter o veículo de Id {Id}.", id);
                throw;
            }
        }

        public async Task<bool> AddVehicle(Vehicle vehicle)
        {
            try
            {
                await _context.Vehicles.AddAsync(vehicle);
                int result = await _context.SaveChangesAsync();
                return result > 0;
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
