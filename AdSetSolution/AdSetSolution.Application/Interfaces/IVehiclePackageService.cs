using AdSetSolution.Application.DTOs;
using AdSetSolution.Application.Utils;
using AdSetSolution.Domain.Models;

namespace AdSetSolution.Application.Interfaces
{
    public interface IVehiclePackageService
    {
        Task<OperationReturn> SetVehiclePackages(IEnumerable<VehiclePackageDTO> vehiclePackagesDTO);
    }
}
