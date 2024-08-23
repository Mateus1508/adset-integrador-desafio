using AdSetSolution.Domain.Enums;
using AdSetSolution.Domain.Models;

namespace AdSetSolution.Domain.Interfaces
{
    public interface IPackageRepository
    {
        Task<Package> GetPackageById(int id);
        Task<IEnumerable<Package>> GetPackageByPortalType(PortalType portalType);
        Task<IEnumerable<Package>> GetAllPackages();
    }
}
