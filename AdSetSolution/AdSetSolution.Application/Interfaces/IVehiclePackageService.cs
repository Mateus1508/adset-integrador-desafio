using AdSetSolution.Application.DTOs;
using AdSetSolution.Application.Utils;

namespace AdSetSolution.Application.Interfaces
{
    public interface IVehiclePackageService
    {
        Task<OperationReturn> GetVehiclePackageByVehicleId(int vehicleId);
        Task<OperationReturn> SetVehiclePackage(VehiclePackageDTO vehiclePackage);
    }
}
