using AdSetSolution.Application.Utils;
using AdSetSolution.Domain.Enums;

namespace AdSetSolution.Application.Interfaces
{
    public interface IPackageService
    {
        Task<OperationReturn> GetPackageById(int id);
        Task<OperationReturn> GetPackageByPortalType(PortalType portalType);
        Task<OperationReturn> GetAllPackages();
    }
}
