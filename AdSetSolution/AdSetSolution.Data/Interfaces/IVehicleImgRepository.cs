using AdSetSolution.Domain.Models;

namespace AdSetSolution.Domain.Interfaces
{
    public interface IVehicleImgRepository
    {
        Task<bool> AddImage(VehicleImg vehicleImg);
        Task<bool> DeleteImage(int id);
    }
}
