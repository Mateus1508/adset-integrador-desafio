using AdSetSolution.Application.Utils;

namespace AdSetSolution.Application.Interfaces
{
    public interface IOptionalService
    {
        Task<OperationReturn> GetAllOptional();
        Task<OperationReturn> GetOptionalById(int id);
    }
}
