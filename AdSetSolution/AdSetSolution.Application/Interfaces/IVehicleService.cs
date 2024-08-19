using AdSetSolution.Application.DTOs;
using AdSetSolution.Application.Utils;

namespace AdSetSolution.Application.Interfaces
{
    public interface IVehicleService
    {
        Task<OperationReturn> GetAllVehicles();
        Task<OperationReturn> GetVehicleById(int id);
        Task<OperationReturn> AddVehicle(VehicleDTO vehicleDto);
        Task<OperationReturn> UpdateVehicle(VehicleDTO vehicleDto);
        Task<OperationReturn> DeleteVehicle(int id);
    }
}
