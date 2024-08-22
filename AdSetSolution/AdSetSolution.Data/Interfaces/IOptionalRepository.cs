using AdSetSolution.Domain.Enums;
using AdSetSolution.Domain.Models;

namespace AdSetSolution.Domain.Interfaces
{
    public interface IOptionalRepository
    {
        Task<Optional> GetOptionalById(int id);
        Task<IEnumerable<Optional>> GetAllOptional();
    }
}
