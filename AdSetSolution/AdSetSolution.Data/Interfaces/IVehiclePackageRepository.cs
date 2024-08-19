using AdSetSolution.Domain.Enums;
using AdSetSolution.Domain.Models;

namespace AdSetSolution.Domain.Interfaces
{
    public interface IVehiclePackageRepository
    {
        Task<VehiclePackage> GetVehiclePackageByVehicleId(int vehicleId);
        Task<VehiclePackage> GetVehiclePackageByVehicleIdAndPortalType(int vehicleId, PortalType portalType);
        Task<bool> AddVehiclePackage(VehiclePackage vehiclePackage);
        Task<bool> UpdateVehiclePackage(VehiclePackage vehiclePackage);
        Task<bool> DeleteVehiclePackage(VehiclePackage vehiclePackage);
    }
}
