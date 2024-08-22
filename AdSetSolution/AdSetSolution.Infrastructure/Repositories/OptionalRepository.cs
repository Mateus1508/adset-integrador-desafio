using AdSetSolution.Domain.Interfaces;
using AdSetSolution.Domain.Models;
using AdSetSolution.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AdSetSolution.Infrastructure.Repositories
{
    public class OptionalRepository : IOptionalRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OptionalRepository> _logger;

        public OptionalRepository(AppDbContext context, ILogger<OptionalRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Optional>> GetAllOptional()
        {
            try
            {
                return await _context.Optional.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os pacotes.");
                throw;
            }
        }

        public async Task<Optional> GetOptionalById(int id)
        {
            try
            {
                return await _context.Optional.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter o pacote de Id {Id}.", id);
                throw;
            }
        }
    }
}
