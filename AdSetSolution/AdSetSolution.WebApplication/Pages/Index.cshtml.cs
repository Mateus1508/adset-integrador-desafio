using AdSetSolution.Application.Interfaces;
using AdSetSolution.Application.DTOs;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdSetSolution.Application.Services;

namespace AdSetSolution.WebApplication.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IVehicleService _vehicleService;
        private readonly IPackageService _packageService;

        public IEnumerable<VehicleDTO> Vehicles { get; private set; }
        public IEnumerable<PackageDTO> Packages { get; private set; }

        public IndexModel(ILogger<IndexModel> logger, IVehicleService vehicleService, IPackageService packageService)
        {
            _logger = logger;
            _vehicleService = vehicleService;
            _packageService = packageService;
        }

        public async Task OnGetAsync()
        {
            var result = await _vehicleService.GetAllVehicles();
            if (result.Success)
            {
                Vehicles = result.Data as IEnumerable<VehicleDTO>;
            }
            else
            {
                _logger.LogError("Erro ao obter veículos: {Message}", result.Message);
                Vehicles = new List<VehicleDTO>();
            }

            var packageResult = await _packageService.GetAllPackages();
            if (packageResult.Success)
            {
                Packages = packageResult.Data as IEnumerable<PackageDTO>;
                Console.WriteLine(Packages);
            }
            else
            {
                _logger.LogError("Erro ao obter pacotes: {Message}", packageResult.Message);
                Packages = Enumerable.Empty<PackageDTO>();
            }
        }
    }
}
