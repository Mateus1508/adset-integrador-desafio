using AdSetSolution.Application.Utils;

namespace AdSetSolution.Application.Interfaces
{
    public interface IVehicleImgService
    {
        Task<OperationReturn> DeleteImage(int id);
    }
}
