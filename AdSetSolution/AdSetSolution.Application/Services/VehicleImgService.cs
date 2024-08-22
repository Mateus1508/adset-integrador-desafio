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
            var operationReturn = new OperationReturn();

            try
            {
                if (id <= 0)
                {
                    operationReturn.Success = false;
                    operationReturn.Message = "ID inválido.";
                    return operationReturn;
                }

                bool isDeleted = await _vehicleImgRepository.DeleteImage(id);

                operationReturn.Success = isDeleted;
                operationReturn.Message = isDeleted ? "Imagem excluída com sucesso." : "Erro ao excluir a imagem.";
            }
            catch (Exception ex)
            {
                operationReturn.Success = false;
                operationReturn.Message = $"Erro ao excluir imagem: {ex.Message}";
            }

            return operationReturn;
        }
    }
}
