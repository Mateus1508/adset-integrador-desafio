using AdSetSolution.Application.DTOs;
using AdSetSolution.Application.Interfaces;
using AdSetSolution.Application.Utils;
using AdSetSolution.Domain.Interfaces;
using AdSetSolution.Domain.Models;
using AutoMapper;

namespace AdSetSolution.Application.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _repository;
        private readonly IVehicleImgRepository _vehicleImgRepository;
        private readonly IVehicleOptionalService _vehicleOptionalService;
        private readonly IMapper _mapper;

        public VehicleService(IVehicleRepository repository, IVehicleImgRepository vehicleImgRepository, IVehicleOptionalService vehicleOptionalService, IMapper mapper)
        {
            _repository = repository;
            _vehicleImgRepository = vehicleImgRepository;
            _vehicleOptionalService = vehicleOptionalService;
            _mapper = mapper;
        }

        public async Task<OperationReturn> GetFilteredVehicles(VehicleFilter filter)
        {
            var operationReturn = new OperationReturn();

            try
            {
                var vehicles = await _repository.GetFilteredVehicles(filter);

                if (vehicles == null || !vehicles.Any())
                {
                    operationReturn.Success = false;
                    operationReturn.Message = "Nenhum veículo encontrado.";
                }
                else
                {
                    var vehicleDtos = _mapper.Map<IEnumerable<VehicleDTO>>(vehicles);

                    operationReturn.Success = true;
                    operationReturn.Data = vehicleDtos;
                    operationReturn.Message = "Veículos retornados com sucesso.";
                }
            }
            catch (Exception ex)
            {
                operationReturn.Success = false;
                operationReturn.Message = $"Erro ao retornar veículos: {ex.Message}";
            }

            return operationReturn;
        }

        public async Task<OperationReturn> GetVehicleById(int id)
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

                var vehicle = await _repository.GetVehicleById(id);

                if (vehicle == null)
                {
                    operationReturn.Success = false;
                    operationReturn.Message = $"Veículo com Id {id} não encontrado.";
                }
                else
                {
                    var vehicleDto = _mapper.Map<VehicleDTO>(vehicle);
                    operationReturn.Success = true;
                    operationReturn.Data = vehicleDto;
                    operationReturn.Message = "Veículo retornado com sucesso.";
                }
            }
            catch (Exception ex)
            {
                operationReturn.Success = false;
                operationReturn.Message = $"Erro ao retornar veículo: {ex.Message}";
            }

            return operationReturn;
        }


        public async Task<OperationReturn> AddVehicle(VehicleDTO vehicleDto)
        {
            var operationReturn = new OperationReturn();

            try
            {
                var vehicle = _mapper.Map<Vehicle>(vehicleDto);

                int vehicleAddedId = await _repository.AddVehicle(vehicle);

                if (vehicleAddedId > 0)
                {
                    if (vehicleDto.VehicleImgs.Count > 0)
                    {
                        bool allImagesAdded = true;

                        foreach (var vehicleImgDto in vehicleDto.VehicleImgs)
                        {
                            if (vehicleImgDto.ImageData != null && vehicleImgDto.ImageData.Length > 0)
                            {
                                vehicleImgDto.VehicleId = vehicleAddedId;
                                var image = _mapper.Map<VehicleImg>(vehicleImgDto);
                                bool imageAdded = await _vehicleImgRepository.AddImage(image);

                                if (!imageAdded)
                                {
                                    allImagesAdded = false;
                                }
                            }
                        }

                        if (!allImagesAdded)
                        {
                            operationReturn.Message = "Veículo adicionado, mas houve erros ao adicionar algumas imagens.";
                        }
                        else
                        {
                            operationReturn.Message = "Veículo e imagens adicionados com sucesso.";
                        }
                    }
                    else
                    {
                        operationReturn.Message = "Veículo adicionado com sucesso, sem imagens.";
                    }

                    if (vehicleDto.OptionalIds != null && vehicleDto.OptionalIds.Any())
                    {
                        var vehicleOptional = vehicleDto.OptionalIds.Select(id => new VehicleOptionalDTO
                        {
                            OptionalId = id,
                            VehicleId = vehicleAddedId
                        }).ToList();

                        var optionalsOperationReturn = await _vehicleOptionalService.SetVehicleOptional(vehicleOptional);

                        if (optionalsOperationReturn.Success)
                        {
                            operationReturn.Message = "Veículo, imagens e opcionais adicionados com sucesso.";
                            operationReturn.Success = true;
                        }
                        else
                        {
                            operationReturn.Message = "Veículo adicionado, mas houve erros ao adicionar alguns opcionais.";
                            operationReturn.Success = false;
                        }
                    }
                    else
                    {
                        operationReturn.Message = "Veículo adicionado com sucesso, sem opcionais.";
                        operationReturn.Success = true;
                    }
                }
                else
                {
                    operationReturn.Success = false;
                    operationReturn.Message = "Falha ao adicionar o veículo.";
                }
            }
            catch (Exception ex)
            {
                operationReturn.Success = false;
                operationReturn.Message = $"Erro ao adicionar veículo: {ex.Message}";
            }

            return operationReturn;
        }



        public async Task<OperationReturn> UpdateVehicle(VehicleDTO vehicleDto)
        {
            var operationReturn = new OperationReturn();

            if (vehicleDto == null)
            {
                operationReturn.Success = false;
                operationReturn.Message = "Veículo inválido.";
                return operationReturn;
            }

            try
            {
                var vehicle = _mapper.Map<Vehicle>(vehicleDto);
                bool isVehicleUpdated = await _repository.UpdateVehicle(vehicle);

                if (isVehicleUpdated)
                {
                    if (vehicleDto.VehicleImgs.Count > 0)
                    {
                        bool allImagesAdded = true;

                        foreach (var vehicleImgDto in vehicleDto.VehicleImgs)
                        {
                            if (vehicleImgDto.ImageData != null && vehicleImgDto.ImageData.Length > 0)
                            {
                                vehicleImgDto.VehicleId = vehicleDto.Id;
                                var image = _mapper.Map<VehicleImg>(vehicleImgDto);
                                bool imageAdded = await _vehicleImgRepository.AddImage(image);

                                if (!imageAdded)
                                {
                                    allImagesAdded = false;
                                }
                            }
                        }

                        if (!allImagesAdded)
                        {
                            operationReturn.Message = "Veículo atualizado, mas houve erros ao adicionar algumas imagens.";
                        }
                        else
                        {
                            operationReturn.Message = "Veículo e imagens atualizados com sucesso.";
                        }
                    }
                    else
                    {
                        operationReturn.Message = "Veículo atualizado com sucesso, sem imagens.";
                    }

                    if (vehicleDto.OptionalIds != null && vehicleDto.OptionalIds.Any())
                    {
                        var vehicleOptional = vehicleDto.OptionalIds.Select(id => new VehicleOptionalDTO
                        {
                            OptionalId = id,
                            VehicleId = vehicle.Id,
                        }).ToList();

                        var optionalsOperationReturn = await _vehicleOptionalService.SetVehicleOptional(vehicleOptional);

                        if (optionalsOperationReturn.Success)
                        {
                            operationReturn.Message = "Veículo, imagens e opcionais adicionados com sucesso.";
                            operationReturn.Success = true;
                        }
                        else
                        {
                            operationReturn.Message = "Veículo adicionado, mas houve erros ao adicionar alguns opcionais.";
                            operationReturn.Success = false;
                        }
                    }
                    else
                    {
                        operationReturn.Message = "Veículo adicionado com sucesso, sem opcionais.";
                        operationReturn.Success = true;
                    }

                    operationReturn.Success = true;
                }
                else
                {
                    operationReturn.Success = false;
                    operationReturn.Message = "Falha ao atualizar o veículo.";
                }
            }
            catch (Exception ex)
            {
                operationReturn.Success = false;
                operationReturn.Message = $"Erro ao atualizar veículo: {ex.Message}";
            }

            return operationReturn;
        }


        public async Task<OperationReturn> DeleteVehicle(int id)
        {
            var operationReturn = new OperationReturn();

            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("ID inválido", nameof(id));
                }

                bool isDeleted = await _repository.DeleteVehicle(id);

                operationReturn.Success = isDeleted;
                operationReturn.Message = isDeleted ? "Veículo excluído com sucesso." : "Erro ao excluir veículo.";
            }
            catch (Exception ex)
            {
                operationReturn.Success = false;
                operationReturn.Message = $"Erro ao excluir veículo: {ex.Message}";
            }

            return operationReturn;
        }

    }
}
