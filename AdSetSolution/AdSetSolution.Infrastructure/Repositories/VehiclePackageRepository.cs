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

        public async Task<VehiclePackage> GetVehiclePackageByVehicleId(int vehicleId)
        {
            try
            {
                return await _context.VehiclePackages.FirstOrDefaultAsync(vp => vp.VehicleId == vehicleId);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter o pacote do veículo com Id {id}.", vehicleId);
                throw;
            }
        }

        public async Task<VehiclePackage> GetVehiclePackageByVehicleIdAndPortalType(int vehicleId, PortalType portalType)
        {
            try
            {
                return await _context.VehiclePackages.FirstOrDefaultAsync(vp => vp.VehicleId == vehicleId && vp.PortalType == portalType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter o pacote do veículo com Id {id}.", vehicleId);
                throw;
            }
        }

        public async Task<bool> AddVehiclePackage(VehiclePackage vehiclePackage)
        {
            try
            {
                await _context.VehiclePackages.AddAsync(vehiclePackage);
                int result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar pacote ao veículo.");
                throw;
            }
        }

        public async Task<bool> UpdateVehiclePackage(VehiclePackage vehiclePackage)
        {
            try
            {
                _context.VehiclePackages.Update(vehiclePackage);
                int result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar pacote do veículo com Id {id}.", vehiclePackage.VehicleId);
                throw;
            }
        }

        public async Task<bool> DeleteVehiclePackage(VehiclePackage vehiclePackage)
        {
            try
            {
                var getVehiclePackage = await _context.VehiclePackages
                    .FirstOrDefaultAsync(vp => vp.VehicleId == vehiclePackage.VehicleId && vp.PackageId == vehiclePackage.PackageId && vp.PortalType == vehiclePackage.PortalType);

                if (getVehiclePackage != null)
                {
                    _context.VehiclePackages.Remove(getVehiclePackage);
                    int result = await _context.SaveChangesAsync();
                    return result > 0;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir pacote do veículo com Id {id} e Pacote com Id {PackageId}.", vehiclePackage.VehicleId, vehiclePackage.PackageId);
                throw;
            }
        }
    }
}
