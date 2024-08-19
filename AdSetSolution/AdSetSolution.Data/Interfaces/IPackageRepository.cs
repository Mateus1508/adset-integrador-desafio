using AdSetSolution.Domain.Enums;
using AdSetSolution.Domain.Models;

namespace AdSetSolution.Domain.Interfaces
{
    public interface IPackageRepository
    {
        Task<Package> GetPackageById(int id);
        Task<Package> GetPackageByPortalType(PortalType portalType);
        Task<IEnumerable<Package>> GetAllPackages();
        Task<bool> UpdatePackage(int packageId, UpdatePackageOperationType operation);
    }
}
