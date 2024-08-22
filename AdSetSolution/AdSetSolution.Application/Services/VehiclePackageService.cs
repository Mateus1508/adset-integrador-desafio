using AdSetSolution.Application.DTOs;
using AdSetSolution.Application.Interfaces;
using AdSetSolution.Application.Utils;
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

        public async Task<OperationReturn> SetVehiclePackages(IEnumerable<VehiclePackageDTO> vehiclePackagesDTO)
        {
            var operationReturn = new OperationReturn();

            try
            {
                var vehiclePackages = _mapper.Map<IEnumerable<VehiclePackage>>(vehiclePackagesDTO);

                var vehicleId = vehiclePackages.First().VehicleId;

                var existingVehiclePackages = await _vehiclePackageRepository.GetVehiclePackagesByVehicleId(vehicleId);

                var packagesToRemove = existingVehiclePackages
                    .Where(existing => !vehiclePackages.Any(vp =>
                        vp.PackageId == existing.PackageId && vp.PortalType == existing.PortalType))
                    .ToList();

                if (packagesToRemove.Any())
                {
                    await _vehiclePackageRepository.DeleteVehiclePackage(packagesToRemove);
                }

                var newPackages = vehiclePackages
                    .Where(vp => !existingVehiclePackages.Any(existing =>
                        existing.PackageId == vp.PackageId && existing.PortalType == vp.PortalType))
                    .ToList();

                if (newPackages.Any())
                {
                    await _vehiclePackageRepository.AddVehiclePackage(newPackages);
                }

                operationReturn.Success = true;
                operationReturn.Message = "Pacotes de veículos atualizados com sucesso.";
            }
            catch (Exception ex)
            {
                operationReturn.Success = false;
                operationReturn.Message = $"Erro ao atualizar pacotes de veículos: {ex.Message}";
            }

            return operationReturn;
        }
    }
}
