using AdSetSolution.Application.DTOs;
using AdSetSolution.Application.Interfaces;
using AdSetSolution.Application.Utils;
using AdSetSolution.Domain.Enums;
using AdSetSolution.Domain.Interfaces;
using AdSetSolution.Domain.Models;
using AutoMapper;

namespace AdSetSolution.Application.Services
{
    public class VehiclePackageService : IVehiclePackageService
    {
        private readonly IVehiclePackageRepository _vehiclePackageRepository;
        private readonly IPackageRepository _packageRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IMapper _mapper;

        public VehiclePackageService(IVehiclePackageRepository vehiclePackageRepository, IVehicleRepository vehicleRepository, IPackageRepository packageRepository, IMapper mapper)
        {
            _vehiclePackageRepository = vehiclePackageRepository;
            _packageRepository = packageRepository;
            _vehicleRepository = vehicleRepository;
            _mapper = mapper;
        }

        public async Task<OperationReturn> GetVehiclePackageByVehicleId(int vehicleId)
        {
            if (vehicleId <= 0)
            {
                return new OperationReturn
                {
                    Success = false,
                    Message = "ID do veículo inválido."
                };
            }

            try
            {
                var vehiclePackage = await _vehiclePackageRepository.GetVehiclePackageByVehicleId(vehicleId);
                if (vehiclePackage == null)
                {
                    return new OperationReturn
                    {
                        Success = false,
                        Message = "Pacote de veículo não encontrado."
                    };
                }

                var vehiclePackageDto = _mapper.Map<VehiclePackageDTO>(vehiclePackage);

                return new OperationReturn
                {
                    Success = true,
                    Data = vehiclePackageDto,
                    Message = "Pacote de veículo recuperado com sucesso."
                };
            }
            catch (Exception ex)
            {
                return new OperationReturn
                {
                    Success = false,
                    Message = $"Erro ao recuperar pacote de veículo: {ex.Message}"
                };
            }
        }

        public async Task<OperationReturn> SetVehiclePackage(VehiclePackageDTO vehiclePackageDto)
        {
            if (vehiclePackageDto == null)
                return new OperationReturn
                {
                    Success = false,
                    Message = "Dados do pacote do veículo não podem ser nulos."
                };

            try
            {
                var vehicle = await _vehicleRepository.GetVehicleById(vehiclePackageDto.VehicleId);
                if (vehicle == null)
                    return new OperationReturn
                    {
                        Success = false,
                        Message = "Veículo não encontrado."
                    };

                var existingPackage = await _vehiclePackageRepository.GetVehiclePackageByVehicleIdAndPortalType(vehiclePackageDto.VehicleId, vehiclePackageDto.PortalType);
                var packageId = vehiclePackageDto.PackageId;  

                if (existingPackage != null)
                {
                    if (packageId == null)
                    {
                        bool deleteResult = await _vehiclePackageRepository.DeleteVehiclePackage(existingPackage);
                        if (deleteResult)
                        {
                            var incrementResult = await UpdatePackageUsedCountAsync(existingPackage.PackageId, UpdatePackageOperationType.Increment);
                            if (!incrementResult.Success)
                                return incrementResult;
                        }

                        return new OperationReturn
                        {
                            Success = deleteResult,
                            Message = deleteResult ? "Pacote do veículo excluído com sucesso." : "Erro ao excluir o pacote do veículo."
                        };
                    }

                    var updatedPackage = _mapper.Map<VehiclePackage>(vehiclePackageDto);
                    bool updateResult = await _vehiclePackageRepository.UpdateVehiclePackage(updatedPackage);
                    if (updateResult)
                    {
                        var incrementResult = await UpdatePackageUsedCountAsync(existingPackage.PackageId, UpdatePackageOperationType.Increment);
                        if (!incrementResult.Success)
                            return incrementResult;

                        var decrementResult = await UpdatePackageUsedCountAsync(packageId.Value, UpdatePackageOperationType.Decrement);
                        if (!decrementResult.Success)
                            return decrementResult;
                    }

                    return new OperationReturn
                    {
                        Success = updateResult,
                        Message = updateResult ? "Pacote do veículo atualizado com sucesso." : "Erro ao atualizar o pacote do veículo."
                    };
                }

                var newPackage = _mapper.Map<VehiclePackage>(vehiclePackageDto);
                bool addResult = await _vehiclePackageRepository.AddVehiclePackage(newPackage);
                if (addResult)
                {
                    var decrementResult = await UpdatePackageUsedCountAsync(newPackage.PackageId, UpdatePackageOperationType.Decrement);
                    if (!decrementResult.Success)
                        return decrementResult;
                }

                return new OperationReturn
                {
                    Success = addResult,
                    Message = addResult ? "Pacote do veículo adicionado com sucesso." : "Erro ao adicionar o pacote do veículo."
                };
            }
            catch (Exception ex)
            {
                return new OperationReturn
                {
                    Success = false,
                    Message = $"Erro ao definir o pacote do veículo: {ex.Message}"
                };
            }
        }

        async Task<OperationReturn> UpdatePackageUsedCountAsync(int packageId, UpdatePackageOperationType operation)
        {
            bool updateResult = await _packageRepository.UpdatePackage(packageId, operation);
            if (!updateResult)
            {
                return new OperationReturn
                {
                    Success = false,
                    Message = $"Erro ao atualizar o campo 'Used' do pacote ({operation})."
                };
            }
            return new OperationReturn { Success = true };
        }
    }
}
