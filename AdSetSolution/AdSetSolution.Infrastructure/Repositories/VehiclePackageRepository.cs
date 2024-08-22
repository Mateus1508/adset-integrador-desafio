using AdSetSolution.Domain.Enums;
using AdSetSolution.Domain.Interfaces;
using AdSetSolution.Domain.Models;
using AdSetSolution.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdSetSolution.Infrastructure.Repositories
{
    public class VehiclePackageRepository : IVehiclePackageRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<VehiclePackageRepository> _logger;

        public VehiclePackageRepository(AppDbContext context, ILogger<VehiclePackageRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<VehiclePackage>> GetVehiclePackagesByVehicleId(int vehicleId)
        {
            return await _context.VehiclePackages
                .Where(vp => vp.VehicleId == vehicleId)
                .ToListAsync();
        }

        public async Task<bool> AddVehiclePackage(IEnumerable<VehiclePackage> vehiclePackages)
        {
            try
            {
                await _context.VehiclePackages.AddRangeAsync(vehiclePackages);
                int result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar pacote ao veículo.");
                throw;
            }
        }

        public async Task<bool> DeleteVehiclePackage(IEnumerable<VehiclePackage> vehiclePackages)
        {
            try
            {
                _context.VehiclePackages.RemoveRange(vehiclePackages);
                int result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                var vehiclePackageIds = string.Join(", ", vehiclePackages.Select(vp => $"VehicleId: {vp.VehicleId}, PackageId: {vp.PackageId}"));
                _logger.LogError(ex, "Erro ao excluir os seguintes pacotes de veículos: {VehiclePackages}.", vehiclePackageIds);
                throw;
            }
        }
    }
}
