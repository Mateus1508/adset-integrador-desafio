using AdSetSolution.Domain.Enums;

namespace AdSetSolution.Domain.Interfaces
{
    public interface VehiclePackage
    {
        Task<VehiclePackage> GetVehiclePackageByVehicleId(int vehicleId, PortalType portalType);
        Task Add(VehiclePackage vehiclePackage);
        Task Update(VehiclePackage vehiclePackage);
        Task Delete(int vehicleId, int packageId, PortalType portalType);
    }
}
