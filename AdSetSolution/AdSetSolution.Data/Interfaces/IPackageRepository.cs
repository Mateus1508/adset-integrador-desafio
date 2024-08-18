using AdSetSolution.Domain.Enums;
using AdSetSolution.Domain.Models;

namespace AdSetSolution.Domain.Interfaces
{
    public interface IPackageRepository
    {
        Task<Package> GetPackageById(int id);
        Task<IEnumerable<Package>> GetAllPackages();
        Task<string> UpdatePackage(Package package);
    }
}
