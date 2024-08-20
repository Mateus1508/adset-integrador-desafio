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
        private readonly IMapper _mapper;

        public VehicleService(IVehicleRepository repository, IVehicleImgRepository vehicleImgRepository, IMapper mapper)
        {
            _repository = repository;
            _vehicleImgRepository = vehicleImgRepository;
            _mapper = mapper;
        }

        public async Task<OperationReturn> GetFilteredVehicles(VehicleFilter filter)
        {
            try
            {
                var vehicles = await _repository.GetFilteredVehicles(filter);

                if (vehicles == null || !vehicles.Any())
                {
                    return new OperationReturn
                    {
                        Success = false,
                        Message = "Nenhum veículo encontrado."
                    };
                }

                var vehicleDtos = _mapper.Map<IEnumerable<VehicleDTO>>(vehicles);

                return new OperationReturn
                {
                    Success = true,
                    Data = vehicleDtos,
                    Message = "Veículos retornados com sucesso."
                };
            }
            catch (Exception ex)
            {
                return new OperationReturn
                {
                    Success = false,
                    Message = $"Erro ao retornar veículos: {ex.Message}"
                };
            }
        }

        public async Task<OperationReturn> GetVehicleById(int id)
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
                var vehicle = await _repository.GetVehicleById(id);
                if (vehicle == null)
                {
                    return new OperationReturn
                    {
                        Success = false,
                        Message = $"Veículo com Id {id} não encontrado."
                    };
                }

                var vehicleDto = _mapper.Map<VehicleDTO>(vehicle);
                return new OperationReturn
                {
                    Success = true,
                    Data = vehicleDto,
                    Message = "Veículo retornado com sucesso."
                };
            }
            catch (Exception ex)
            {
                return new OperationReturn
                {
                    Success = false,
                    Message = $"Erro ao retornar veículo: {ex.Message}"
                };
            }
        }

        public async Task<OperationReturn> AddVehicle(VehicleDTO vehicleDto)
        {
            try
            {
                var vehicle = _mapper.Map<Vehicle>(vehicleDto);
                int vehicleAddedId = await _repository.AddVehicle(vehicle);

                var imageOperationResult = new OperationReturn
                {
                    Success = true,
                    Message = "Veículo adicionado com sucesso."
                };

                if (vehicleAddedId > 0 && vehicleDto.VehicleImgs.Count > 0)
                {
                    bool allImagesAdded = true;

                    foreach (var vehicleImgDto in vehicleDto.VehicleImgs)
                    {
                        if (vehicleImgDto.ImageData == null || vehicleImgDto.ImageData.Length == 0)
                        {
                            continue;
                        }

                        vehicleImgDto.VehicleId = vehicleAddedId;
                        var image = _mapper.Map<VehicleImg>(vehicleImgDto);
                        bool imageAdded = await _vehicleImgRepository.AddImage(image);

                        if (!imageAdded)
                        {
                            allImagesAdded = false;
                        }
                    }

                    if (!allImagesAdded)
                    {
                        imageOperationResult.Message = "Veículo adicionado, mas houve erros ao adicionar algumas imagens.";
                    }
                }

                return new OperationReturn
                {
                    Success = vehicleAddedId > 0,
                    Message = imageOperationResult.Message
                };
            }
            catch (Exception ex)
            {
                return new OperationReturn
                {
                    Success = false,
                    Message = $"Erro ao adicionar veículo: {ex.Message}"
                };
            }
        }

        public async Task<OperationReturn> UpdateVehicle(VehicleDTO vehicleDto)
        {
            if (vehicleDto == null)
            {
                return new OperationReturn
                {
                    Success = false,
                    Message = "Veículo inválido."
                };
            }

            try
            {
                var vehicle = _mapper.Map<Vehicle>(vehicleDto);
                bool isVehicleUpdated = await _repository.UpdateVehicle(vehicle);

                var imageOperationResult = new OperationReturn
                {
                    Success = true,
                    Message = "Veículo atualizado com sucesso."
                };

                if (isVehicleUpdated && vehicleDto.VehicleImgs.Count > 0)
                {
                    bool allImagesAdded = true;

                    foreach (var vehicleImgDto in vehicleDto.VehicleImgs)
                    {
                        if (vehicleImgDto.ImageData == null || vehicleImgDto.ImageData.Length == 0)
                        {
                            continue;
                        }

                        vehicleImgDto.VehicleId = vehicleDto.Id;
                        var image = _mapper.Map<VehicleImg>(vehicleImgDto);
                        bool imageAdded = await _vehicleImgRepository.AddImage(image);

                        if (!imageAdded)
                        {
                            allImagesAdded = false;
                        }
                    }

                    if (!allImagesAdded)
                    {
                        imageOperationResult.Message = "Veículo atualizado, mas houve erros ao adicionar algumas imagens.";
                    }
                }

                return new OperationReturn
                {
                    Success = isVehicleUpdated,
                    Message = imageOperationResult.Message
                };
            }
            catch (Exception ex)
            {
                return new OperationReturn
                {
                    Success = false,
                    Message = $"Erro ao atualizar veículo: {ex.Message}"
                };
            }
        }

        public async Task<OperationReturn> DeleteVehicle(int id)
        {
            if (id <= 0) throw new ArgumentException("ID inválido", nameof(id));

            try
            {
                bool isDeleted = await _repository.DeleteVehicle(id);

                return new OperationReturn
                {
                    Success = isDeleted,
                    Message = isDeleted ? "Veículo excluído com sucesso." : "Erro ao excluir veículo."
                };
            }
            catch (Exception ex)
            {
                return new OperationReturn
                {
                    Success = false,
                    Message = $"Erro ao excluir veículo: {ex.Message}"
                };
            }
        }

        private IEnumerable<Vehicle> ApplyFilters(IEnumerable<Vehicle> vehicles, VehicleDTO filter)
        {
            if (!string.IsNullOrEmpty(filter.Marca))
            {
                vehicles = vehicles.Where(v => v.Marca.Contains(filter.Marca, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(filter.Modelo))
            {
                vehicles = vehicles.Where(v => v.Modelo.Contains(filter.Modelo, StringComparison.OrdinalIgnoreCase));
            }

            if (filter.Ano > 0)
            {
                vehicles = vehicles.Where(v => v.Ano == filter.Ano);
            }

            if (!string.IsNullOrEmpty(filter.Placa))
            {
                vehicles = vehicles.Where(v => v.Placa.Contains(filter.Placa, StringComparison.OrdinalIgnoreCase));
            }

            if (filter.Km > 0)
            {
                vehicles = vehicles.Where(v => v.Km == filter.Km);
            }

            if (!string.IsNullOrEmpty(filter.Cor))
            {
                vehicles = vehicles.Where(v => v.Cor.Contains(filter.Cor, StringComparison.OrdinalIgnoreCase));
            }

            if (filter.Preco > 0)
            {
                vehicles = vehicles.Where(v => v.Preco == filter.Preco);
            }

            if (!string.IsNullOrEmpty(filter.Opcionais))
            {
                vehicles = vehicles.Where(v => v.Opcionais.Contains(filter.Opcionais, StringComparison.OrdinalIgnoreCase));
            }

            return vehicles;
        }
    }
}
