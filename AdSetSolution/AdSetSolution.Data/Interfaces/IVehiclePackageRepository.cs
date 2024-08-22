using AdSetSolution.Domain.Enums;
using AdSetSolution.Domain.Models;

namespace AdSetSolution.Domain.Interfaces
{
    public interface IVehiclePackageRepository
    {
        Task<List<VehiclePackage>> GetVehiclePackagesByVehicleId(int vehicleId);
        Task<bool> AddVehiclePackage(IEnumerable<VehiclePackage> vehiclePackages);
        Task<bool> DeleteVehiclePackage(IEnumerable<VehiclePackage> vehiclePackages);
    }
}
