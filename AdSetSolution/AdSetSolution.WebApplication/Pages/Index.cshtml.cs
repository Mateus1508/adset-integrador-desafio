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
        private readonly IVehiclePackageService _vehiclePackageService;

        public IEnumerable<VehicleDTO> Vehicles { get; private set; } = Enumerable.Empty<VehicleDTO>();
        public IEnumerable<PackageDTO> ICarros { get; private set; } = Enumerable.Empty<PackageDTO>();
        public IEnumerable<PackageDTO> WebMotors { get; private set; } = Enumerable.Empty<PackageDTO>();
        public IEnumerable<OptionalDTO> Optional { get; private set; } = Enumerable.Empty<OptionalDTO>();

        public class VehiclePackageFormModel
        {
            public int VehicleId { get; set; }
            public Dictionary<PortalType, int?> Packages { get; set; }
        }

        [BindProperty]
        public List<VehiclePackageFormModel> VehiclePackagesForm { get; set; } = new List<VehiclePackageFormModel>();

        [BindProperty]
        public VehicleFilter Filter { get; set; } = new VehicleFilter();

        [BindProperty]
        public int VehicleIdToDeletion { get; set; }

        public IEnumerable<SelectListItem> OptionalItems { get; set; } = Enumerable.Empty<SelectListItem>();

        public int TotalVehicles { get; set; }
        public int VehiclesWithPhotos { get; set; }
        public int VehiclesWithoutPhotos { get; set; }
        public int PageNumber { get; set; } = 1;
        public int TotalPages { get; set; }
        public int PageGroupSize { get; set; } = 10;
        public int CurrentGroup { get; set; } = 1;
        public int TotalGroups { get; set; } = 1;

        public string AlertMessage { get; set; } = string.Empty;

        public IndexModel(
            ILogger<IndexModel> logger,
            IVehicleService vehicleService,
            IPackageService packageService,
            IOptionalService optionalService,
            IVehiclePackageService vehiclePackageService
        )
        {
            _logger = logger;
            _vehicleService = vehicleService;
            _packageService = packageService;
            _optionalService = optionalService;
            _vehiclePackageService = vehiclePackageService;
        }

        public async Task OnGetAsync()
        {
            if (Request.Query.TryGetValue("page", out var pageValue) && int.TryParse(pageValue, out var page))
            {
                PageNumber = page;
            }

            PageNumber = Math.Max(PageNumber, 1);

            await LoadVehiclesAsync();
            await LoadPackagesByPortalTypeAsync();
            await LoadOptionalAsync();

            TotalGroups = (int)Math.Ceiling(TotalPages / (double)PageGroupSize);
            CurrentGroup = (int)Math.Ceiling(PageNumber / (double)PageGroupSize);

            PageNumber = Math.Min(PageNumber, TotalPages);
        }

        public async Task<IActionResult> OnPostFilterAsync()
        {
            await OnGetAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateVehiclePackagesAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            var vehiclePackageDTOs = VehiclePackagesForm
                .SelectMany(vp => vp.Packages
                    .Where(pp => pp.Value.HasValue)
                    .Select(pp => new VehiclePackageDTO
                    {
                        VehicleId = vp.VehicleId,
                        PackageId = pp.Value,
                        PortalType = pp.Key
                    }))
                .ToList();

            var result = await _vehiclePackageService.SetVehiclePackages(vehiclePackageDTOs);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "Pacotes de veículos atualizados com sucesso.";
                await OnGetAsync();
                return Page();
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
                await OnGetAsync();
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteVehicleAsync()
        {
                var result = await _vehicleService.DeleteVehicle(VehicleIdToDeletion);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = "Pacote de veículo excluído com sucesso.";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                }
            

            await OnGetAsync();
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
                        TotalVehicles = vehicles.Count();

                        TotalPages = (int)Math.Ceiling(TotalVehicles / 1.0);

                        PageNumber = Math.Min(PageNumber, TotalPages);

                        Vehicles = vehicles
                            .Skip((PageNumber - 1) * 1)
                            .Take(1);

                        var vehiclesList = Vehicles.ToList();
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
            var iCarrosResult = await _packageService.GetPackageByPortalType(PortalType.ICarros);
            var webMotorsResult = await _packageService.GetPackageByPortalType(PortalType.WebMotors);
            ICarros = iCarrosResult.Data as IEnumerable<PackageDTO> ?? Enumerable.Empty<PackageDTO>();
            WebMotors = webMotorsResult.Data as IEnumerable<PackageDTO> ?? Enumerable.Empty<PackageDTO>();
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
