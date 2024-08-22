using AdSetSolution.Application.DTOs;
using AdSetSolution.Application.Extensions;
using AdSetSolution.Application.Interfaces;
using AdSetSolution.Application.Utils;
using AdSetSolution.Domain.Enums;
using AdSetSolution.Domain.Interfaces;
using AutoMapper;


namespace AdSetSolution.Application.Services
{
    public class PackageService : IPackageService
    {
        private readonly IPackageRepository _repository;
        private readonly IMapper _mapper;

        public PackageService(IPackageRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationReturn> GetAllPackages()
        {
            var operationReturn = new OperationReturn();

            try
            {
                var packages = await _repository.GetAllPackages();
                var packageDtos = _mapper.Map<IEnumerable<PackageDTO>>(packages);

                operationReturn.Success = true;
                operationReturn.Data = packageDtos;
                operationReturn.Message = "Pacotes retornados com sucesso.";
            }
            catch (Exception ex)
            {
                operationReturn.Success = false;
                operationReturn.Message = $"Erro ao retornar pacotes: {ex.Message}";
            }

            return operationReturn;
        }

        public async Task<OperationReturn> GetPackageById(int id)
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

                var package = await _repository.GetPackageById(id);
                if (package == null)
                {
                    operationReturn.Success = false;
                    operationReturn.Message = $"Pacote com Id {id} não encontrado.";
                    return operationReturn;
                }

                var packageDto = _mapper.Map<PackageDTO>(package);
                operationReturn.Success = true;
                operationReturn.Data = packageDto;
                operationReturn.Message = "Pacote retornado com sucesso.";
                return operationReturn;
            }
            catch (Exception ex)
            {
                operationReturn.Success = false;
                operationReturn.Message = $"Erro ao retornar pacote: {ex.Message}";
                return operationReturn;
            }
        }

        public async Task<OperationReturn> GetPackageByPortalType(PortalType portalType)
        {
            var operationReturn = new OperationReturn();

            if (portalType != PortalType.ICarros && portalType != PortalType.WebMotors)
            {
                operationReturn.Success = false;
                operationReturn.Message = "Portal inválido.";
                return operationReturn;
            }

            try
            {
                var package = await _repository.GetPackageByPortalType(portalType);
                if (package == null)
                {
                    operationReturn.Success = false;
                    operationReturn.Message = $"Pacote do portal {portalType.getPortalName()} não encontrado.";
                    return operationReturn;
                }

                var packageDto = _mapper.Map<PackageDTO>(package);
                operationReturn.Success = true;
                operationReturn.Data = packageDto;
                operationReturn.Message = "Pacote retornado com sucesso.";
            }
            catch (Exception ex)
            {
                operationReturn.Success = false;
                operationReturn.Message = $"Erro ao retornar pacote: {ex.Message}";
            }

            return operationReturn;
        }

    }
}
