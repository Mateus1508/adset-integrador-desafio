using AdSetSolution.Domain.Models;

namespace AdSetSolution.Domain.Interfaces
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetAllVehicles();
        Task<Vehicle> GetVehicleById(int id);
        Task<bool> AddVehicle(Vehicle vehicle);
        Task<bool> UpdateVehicle(Vehicle vehicle);
        Task<bool> DeleteVehicle(int id);
    }
}
