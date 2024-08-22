using AdSetSolution.Application.DTOs;
using AdSetSolution.Application.Interfaces;
using AdSetSolution.Application.Utils;
using AdSetSolution.Domain.Interfaces;
using AutoMapper;

namespace AdSetSolution.Application.Services
{
    public class OptionalService : IOptionalService
    {
        private readonly IOptionalRepository _repository;
        private readonly IMapper _mapper;

        public OptionalService(IOptionalRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

        }

        public async Task<OperationReturn> GetAllOptional()
        {
            var operationReturn = new OperationReturn();

            try
            {
                var optional = await _repository.GetAllOptional();
                var optionalDTO = _mapper.Map<IEnumerable<OptionalDTO>>(optional);

                operationReturn.Success = true;
                operationReturn.Data = optionalDTO;
                operationReturn.Message = "Opcionais retornados com sucesso.";
            }
            catch (Exception ex)
            {
                operationReturn.Success = false;
                operationReturn.Message = $"Erro ao retornar opcionais: {ex.Message}";
            }

            return operationReturn;
        }

        public async Task<OperationReturn> GetOptionalById(int id)
        {
            var operationReturn = new OperationReturn();

            try
            {
                if (id <= 0)
                {
                    operationReturn.Success = false;
                    operationReturn.Message = "Id inválido.";
                    return operationReturn;
                }

                var optional = await _repository.GetOptionalById(id);
                if (optional == null)
                {
                    operationReturn.Success = false;
                    operationReturn.Message = $"Opcional com Id {id} não encontrado.";
                    return operationReturn;
                }

                var optionalDTO = _mapper.Map<OptionalDTO>(optional);
                operationReturn.Success = true;
                operationReturn.Data = optionalDTO;
                operationReturn.Message = "Opcional retornado com sucesso.";
                return operationReturn;
            }
            catch (Exception ex)
            {
                operationReturn.Success = false;
                operationReturn.Message = $"Erro ao retornar pacote: {ex.Message}";
                return operationReturn;
            }
        }
    }
}
