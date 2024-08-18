using AdSetSolution.Domain.Models;

namespace AdSetSolution.Domain.Interfaces
{
    public interface VehicleImgRepository
    {
        Task<string> AddImage(VehicleImg vehicleImg);
        Task<bool> DeleteImage(int id);
    }
}
