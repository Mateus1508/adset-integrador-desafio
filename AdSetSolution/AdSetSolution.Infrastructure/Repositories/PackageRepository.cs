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
    }
}
