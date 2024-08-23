using AdSetSolution.Domain.Interfaces;
using AdSetSolution.Domain.Models;
using AdSetSolution.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AdSetSolution.Infrastructure.Repositories
{
    public class VehicleImgRepository : IVehicleImgRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<VehicleImgRepository> _logger;

        public VehicleImgRepository(AppDbContext context, ILogger<VehicleImgRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddImage(VehicleImg vehicleImg)
        {
            try
            {
                var existingImage = await _context.VehicleImgs
                    .FirstOrDefaultAsync(img => img.FileName == vehicleImg.FileName && img.VehicleId == vehicleImg.VehicleId);

                if (existingImage != null)
                {
                    return false;
                }

                _context.VehicleImgs.Add(vehicleImg);
                int result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar imagem do veículo.");
                throw;
            }
        }

        public async Task<bool> DeleteImage(int id)
        {
            try
            {
                var vehicleImg = await _context.VehicleImgs.AsNoTracking().FirstOrDefaultAsync(img => img.Id == id);
                if (vehicleImg != null)
                {
                    _context.VehicleImgs.Remove(vehicleImg);
                    int result = await _context.SaveChangesAsync();
                    return result > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir imagem do veículo com Id {Id}.", id);
                throw;
            }
        }
    }
}
