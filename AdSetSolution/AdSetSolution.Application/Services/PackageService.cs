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
            try
            {
                var packages = await _repository.GetAllPackages();
                var packageDtos = _mapper.Map<IEnumerable<PackageDTO>>(packages);

                return new OperationReturn
                {
                    Success = true,
                    Data = packageDtos,
                    Message = "Pacotes retornados com sucesso."
                };
            }
            catch (Exception ex)
            {
                return new OperationReturn
                {
                    Success = false,
                    Message = $"Erro ao retornar pacotes: {ex.Message}"
                };
            }
        }

        public async Task<OperationReturn> GetPackageById(int id)
        {
            if (id <= 0)
            {
                return new OperationReturn
                {
                    Success = false,
                    Message = "Id inválido."
                };
            }

            try
            {
                var package = await _repository.GetPackageById(id);
                if (package == null)
                {
                    return new OperationReturn
                    {
                        Success = false,
                        Message = $"Pacote com Id {id} não encontrado."
                    };
                }

                var packageDto = _mapper.Map<PackageDTO>(package);
                return new OperationReturn
                {
                    Success = true,
                    Data = packageDto,
                    Message = "Pacote retornado com sucesso."
                };
            }
            catch (Exception ex)
            {
                return new OperationReturn
                {
                    Success = false,
                    Message = $"Erro ao retornar pacote: {ex.Message}"
                };
            }
        }

        public async Task<OperationReturn> GetPackageByPortalType(PortalType portalType)
        {
            if (portalType != PortalType.ICarros || portalType != PortalType.WebMotors)
            {
                return new OperationReturn
                {
                    Success = false,
                    Message = "Portal inválido."
                };
            }

            try
            {
                var package = await _repository.GetPackageByPortalType(portalType);
                if (package == null)
                {
                    return new OperationReturn
                    {
                        Success = false,
                        Message = $"Pacote do portal {portalType.getPortalName()} não encontrado."
                    };
                }

                var packageDto = _mapper.Map<PackageDTO>(package);
                return new OperationReturn
                {
                    Success = true,
                    Data = packageDto,
                    Message = "Pacote retornado com sucesso."
                };
            }
            catch (Exception ex)
            {
                return new OperationReturn
                {
                    Success = false,
                    Message = $"Erro ao retornar pacote: {ex.Message}"
                };
            }
        }
    }
}
