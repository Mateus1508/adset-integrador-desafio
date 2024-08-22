using AdSetSolution.Domain.Models;

namespace AdSetSolution.Domain.Interfaces
{
    public interface IVehicleOptionalRepository
    {
        Task<List<VehicleOptional>> GetVehicleOptionalsByVehicleId(int vehicleId);
        Task<bool> AddVehicleOptional(IEnumerable<VehicleOptional> vehicleOptional);
        Task<bool> DeleteVehicleOptional(IEnumerable<VehicleOptional> vehicleOptional);
    }
}
