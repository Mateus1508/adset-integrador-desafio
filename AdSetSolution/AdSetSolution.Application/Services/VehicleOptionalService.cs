using AdSetSolution.Application.DTOs;
using AdSetSolution.Application.Interfaces;
using AdSetSolution.Application.Utils;
using AdSetSolution.Domain.Interfaces;
using AdSetSolution.Domain.Models;
using AutoMapper;

namespace AdSetSolution.Application.Services
{
    public class VehicleOptionalService : IVehicleOptionalService
    {
        private readonly IVehicleOptionalRepository _vehicleOptionalRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IOptionalRepository _optionalRepository;
        private readonly IMapper _mapper;

        public VehicleOptionalService(IVehicleOptionalRepository vehicleOptionalRepository, IVehicleRepository vehicleRepository, IOptionalRepository optionalRepository, IMapper mapper)
        {
            _vehicleOptionalRepository = vehicleOptionalRepository;
            _vehicleRepository = vehicleRepository;
            _optionalRepository = optionalRepository;
            _mapper = mapper;
        }

        public async Task<OperationReturn> SetVehicleOptional(IEnumerable<VehicleOptionalDTO> vehicleOptionalsDTO)
        {
            var operationReturn = new OperationReturn();

            try
            {
                var vehicleOptionals = _mapper.Map<IEnumerable<VehicleOptional>>(vehicleOptionalsDTO);
                var vehicleId = vehicleOptionals.First().VehicleId;

                await _vehicleOptionalRepository.DeleteVehicleOptional(vehicleId);

                await _vehicleOptionalRepository.AddVehicleOptional(vehicleOptionals);

                operationReturn.Success = true;
                operationReturn.Message = "Opcionais de veículos atualizados com sucesso.";
            }
            catch (Exception ex)
            {
                operationReturn.Success = false;
                operationReturn.Message = $"Erro ao atualizar opcionais de veículos: {ex.Message}";
            }

            return operationReturn;
        }
    }
}
