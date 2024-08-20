using AdSetSolution.Domain.Models;

namespace AdSetSolution.Domain.Interfaces
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetFilteredVehicles(VehicleFilter filter);
        Task<Vehicle> GetVehicleById(int id);
        Task<int> AddVehicle(Vehicle vehicle);
        Task<bool> UpdateVehicle(Vehicle vehicle);
        Task<bool> DeleteVehicle(int id);
    }
}
