using AdSetSolution.Domain.Interfaces;
using AdSetSolution.Domain.Models;
using AdSetSolution.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class VehicleOptionalRepository : IVehicleOptionalRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<VehicleOptionalRepository> _logger;

    public VehicleOptionalRepository(AppDbContext context, ILogger<VehicleOptionalRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<VehicleOptional>> GetVehicleOptionalsByVehicleId(int vehicleId)
    {
        try
        {
            return await _context.VehicleOptional
                .Where(vo => vo.VehicleId == vehicleId)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter opcionais do veículo com Id {VehicleId}.", vehicleId);
            throw;
        }
    }

    public async Task<bool> AddVehicleOptional(IEnumerable<VehicleOptional> vehicleOptional)
    {
        try
        {
            await _context.VehicleOptional.AddRangeAsync(vehicleOptional);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar opcionais ao veículo.");
            throw;
        }
    }

    public async Task<bool> DeleteVehicleOptional(IEnumerable<VehicleOptional> vehicleOptional)
    {
        try
        {
            var existingOptionals = await _context.VehicleOptional
                .Where(vo => vehicleOptional.Select(vp => new { vp.VehicleId, vp.OptionalId })
                                             .Contains(new { vo.VehicleId, vo.OptionalId }))
                .ToListAsync();

            if (!existingOptionals.Any())
            {
                _logger.LogInformation("Nenhum opcional encontrado para exclusão.");
                return false;
            }

            _context.VehicleOptional.RemoveRange(existingOptionals);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir opcionais do veículo.");
            throw;
        }
    }
}
