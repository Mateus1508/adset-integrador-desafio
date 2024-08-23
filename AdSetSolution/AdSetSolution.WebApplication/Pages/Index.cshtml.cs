using AdSetSolution.Application.Interfaces;
using AdSetSolution.Application.DTOs;
using AdSetSolution.Domain.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using AdSetSolution.Domain.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdSetSolution.WebApplication.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IVehicleService _vehicleService;
        private readonly IPackageService _packageService;
        private readonly IOptionalService _optionalService;

        public IEnumerable<VehicleDTO> Vehicles { get; private set; } = Enumerable.Empty<VehicleDTO>();
        public IEnumerable<PackageDTO> ICarros { get; private set; } = Enumerable.Empty<PackageDTO>();
        public IEnumerable<PackageDTO> WebMotors { get; private set; } = Enumerable.Empty<PackageDTO>();
        public IEnumerable<OptionalDTO> Optional { get; private set; } = Enumerable.Empty<OptionalDTO>();
        [BindProperty]
        public VehicleFilter Filter { get; set; } = new VehicleFilter();
        public IEnumerable<SelectListItem> OptionalItems { get; set; } = Enumerable.Empty<SelectListItem>();

        public int TotalVehicles { get; set; }
        public int VehiclesWithPhotos { get; set; }
        public int VehiclesWithoutPhotos { get; set; }

        public string AlertMessage { get; set; } = string.Empty;

        public IndexModel(ILogger<IndexModel> logger, IVehicleService vehicleService, IPackageService packageService, IOptionalService optionalService)
        {
            _logger = logger;
            _vehicleService = vehicleService;
            _packageService = packageService;
            _optionalService = optionalService;
        }

        public async Task OnGetAsync()
        {
            await LoadPackagesByPortalTypeAsync();
            await LoadVehiclesAsync();
            await LoadOptionalAsync();
        }

        public async Task<IActionResult> OnPostFilterAsync()
        {
            await LoadVehiclesAsync();
            await LoadOptionalAsync();
            await LoadPackagesByPortalTypeAsync();
            return Page();
        }

        private async Task LoadVehiclesAsync()
        {
            try
            {
                var vehicleResult = await _vehicleService.GetFilteredVehicles(Filter);
                if (vehicleResult.Success)
                {
                    var vehicles = vehicleResult.Data as IEnumerable<VehicleDTO>;
                    if (vehicles != null && vehicles.Any())
                    {
                        Vehicles = vehicles;
                        var vehiclesList = Vehicles.ToList();
                        TotalVehicles = vehiclesList.Count;
                        VehiclesWithPhotos = vehiclesList.Count(v => v.VehicleImgs != null && v.VehicleImgs.Any());
                        VehiclesWithoutPhotos = TotalVehicles - VehiclesWithPhotos;

                        AlertMessage = string.Empty;
                    }
                    else
                    {
                        Vehicles = Enumerable.Empty<VehicleDTO>();
                        AlertMessage = "Nenhum veículo encontrado.";
                    }
                }
                else
                {
                    _logger.LogError("Erro ao obter veículos: {Message}", vehicleResult.Message);
                    Vehicles = Enumerable.Empty<VehicleDTO>();
                    AlertMessage = "Erro ao obter veículos. Por favor, tente novamente.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao filtrar veículos.");
                Vehicles = Enumerable.Empty<VehicleDTO>();
                AlertMessage = "Erro ao filtrar veículos. Por favor, tente novamente.";
            }
        }

        private async Task LoadPackagesByPortalTypeAsync()
        {
            var packageResult = await _packageService.GetAllPackages();
            if (packageResult.Success && packageResult.Data != null)
            {
                var packages = packageResult.Data as IEnumerable<PackageDTO> ?? Enumerable.Empty<PackageDTO>();

                ICarros = packages.Where(p => p.PortalType == PortalType.ICarros).ToList();
                WebMotors = packages.Where(p => p.PortalType == PortalType.WebMotors).ToList();
            }
            else
            {
                _logger.LogError("Erro ao obter pacotes: {Message}", packageResult.Message);
                ICarros = Enumerable.Empty<PackageDTO>();
                WebMotors = Enumerable.Empty<PackageDTO>();
            }
        }

        private async Task LoadOptionalAsync()
        {
            try
            {
                var optionalResult = await _optionalService.GetAllOptional();
                if (optionalResult.Success)
                {
                    Optional = optionalResult.Data as IEnumerable<OptionalDTO> ?? Enumerable.Empty<OptionalDTO>();
                    OptionalItems = Optional.Select(opt => new SelectListItem
                    {
                        Value = opt.Id.ToString(),
                        Text = opt.Name
                    }).ToList();
                }
                else
                {
                    _logger.LogError("Erro ao obter opcionais: {Message}", optionalResult.Message);
                    Optional = Enumerable.Empty<OptionalDTO>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar opcionais.");
                Optional = Enumerable.Empty<OptionalDTO>();
            }
        }
    }
}
