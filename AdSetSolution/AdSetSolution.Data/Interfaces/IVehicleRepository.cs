using AdSetSolution.Domain.Models;

namespace AdSetSolution.Domain.Interfaces
{
    public interface IVehicleRepository
    {
        Task<Vehicle> GetById(int id);
        Task<IEnumerable<Vehicle>> GetAll();
        Task Add(Vehicle vehicle);
        Task Update(Vehicle vehicle);
        Task Delete(int id);
    }
}
