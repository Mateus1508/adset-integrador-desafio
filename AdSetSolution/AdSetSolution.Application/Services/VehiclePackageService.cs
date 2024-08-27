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

                var packagesToAlter = new List<VehiclePackage>();

                foreach (var package in vehiclePackages)
                {
                    if (package.PackageId > 0 && package.VehicleId > 0)
                    {
                        packagesToAlter.Add(package);
                    }
                }

                var groupedPackages = packagesToAlter.GroupBy(p => p.VehicleId);

                foreach (var group in groupedPackages)
                {
                    var vehicleId = group.Key;

                    var existingVehiclePackages = await _vehiclePackageRepository.GetVehiclePackagesByVehicleId(vehicleId);

                    if (existingVehiclePackages.Any())
                    {
                        await _vehiclePackageRepository.DeleteVehiclePackage(existingVehiclePackages);
                    }

                    await _vehiclePackageRepository.AddVehiclePackage(group.ToList());
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
