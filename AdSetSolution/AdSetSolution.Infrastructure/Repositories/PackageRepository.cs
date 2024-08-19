using AdSetSolution.Domain.Enums;
using AdSetSolution.Domain.Interfaces;
using AdSetSolution.Domain.Models;
using AdSetSolution.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AdSetSolution.Infrastructure.Repositories
{
    public class PackageRepository : IPackageRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PackageRepository> _logger;

        public PackageRepository(AppDbContext context, ILogger<PackageRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Package>> GetAllPackages()
        {
            try
            {
                return await _context.Packages.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os pacotes.");
                throw;
            }
        }

        public async Task<Package> GetPackageById(int id)
        {
            try
            {
                return await _context.Packages.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter o pacote de Id {Id}.", id);
                throw;
            }
        }

        public async Task<Package> GetPackageByPortalType(PortalType portalType)
        {
            try
            {
                return await _context.Packages.FirstOrDefaultAsync(p => p.PortalType == portalType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter o pacote do portal {portalType}.", portalType);
                throw;
            }
        }

        public async Task<bool> UpdatePackage(int packageId, UpdatePackageOperationType operation)
        {
            try
            {
                var package = await _context.Packages.FindAsync(packageId);
                if (package == null)
                {
                    _logger.LogWarning("Pacote com Id {PackageId} não encontrado.", packageId);
                    return false;
                }

                if (operation == UpdatePackageOperationType.Increment)
                {
                    package.Used += 1;
                }
                else if (operation == UpdatePackageOperationType.Decrement)
                {
                    package.Used -= 1;
                }

                _context.Packages.Update(package);
                int result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o pacote com Id {PackageId}.", packageId);
                throw;
            }
        }
    }
}
