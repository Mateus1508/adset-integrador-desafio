using AdSetSolution.Application.DTOs;
using AdSetSolution.Application.Utils;
using AdSetSolution.Domain.Models;

namespace AdSetSolution.Application.Interfaces
{
    public interface IVehicleService
    {
        Task<OperationReturn> GetFilteredVehicles(VehicleFilter filter);
        Task<OperationReturn> GetVehicleById(int id);
        Task<OperationReturn> AddVehicle(VehicleDTO vehicleDto);
        Task<OperationReturn> UpdateVehicle(VehicleDTO vehicleDto);
        Task<OperationReturn> DeleteVehicle(int id);
    }
}
