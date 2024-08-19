using AdSetSolution.Application.Interfaces;
using AdSetSolution.Application.Utils;
using AdSetSolution.Domain.Interfaces;

namespace AdSetSolution.Application.Services
{
    public class VehicleImgService : IVehicleImgService
    {
        private readonly IVehicleImgRepository _vehicleImgRepository;

        public VehicleImgService(IVehicleImgRepository vehicleImgRepository)
        {
            _vehicleImgRepository = vehicleImgRepository;
        }

        public async Task<OperationReturn> DeleteImage(int id)
        {
            if (id <= 0)
            {
                return new OperationReturn
                {
                    Success = false,
                    Message = "ID inválido."
                };
            }

            try
            {
                bool isDeleted = await _vehicleImgRepository.DeleteImage(id);

                return new OperationReturn
                {
                    Success = isDeleted,
                    Message = isDeleted ? "Imagem excluída com sucesso." : "Erro ao excluir a imagem."
                };
            }
            catch (Exception ex)
            {
                return new OperationReturn
                {
                    Success = false,
                    Message = $"Erro ao excluir imagem: {ex.Message}"
                };
            }
        }
    }
}
