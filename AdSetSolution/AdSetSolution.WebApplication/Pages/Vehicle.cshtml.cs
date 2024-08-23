using AdSetSolution.Application.DTOs;
using AdSetSolution.Application.Interfaces;
using AdSetSolution.Application.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdSetSolution.WebApplication.Pages
{
    public class VehicleModel : PageModel
    {
        private readonly ILogger<VehicleModel> _logger;
        private readonly IVehicleService _vehicleService;
        private readonly IVehicleImgService _vehicleImgService;
        private readonly IOptionalService _optionalService;

        [BindProperty]
        public VehicleDTO Vehicle { get; set; } = new VehicleDTO();

        [BindProperty]
        public List<IFormFile> VehicleImgs { get; set; } = new List<IFormFile>();

        public bool IsEditing { get; set; }

        public IEnumerable<OptionalDTO> Optional { get; private set; } = Enumerable.Empty<OptionalDTO>();
        public IEnumerable<SelectListItem> OptionalItems { get; set; } = Enumerable.Empty<SelectListItem>();

        [BindProperty]
        public IEnumerable<int> OptionalSelectedItems { get; set; } = Enumerable.Empty<int>();

        public IEnumerable<VehicleImgDTO> ExistingImages { get; set; } = new List<VehicleImgDTO>();

        public string Message { get; set; }

        public VehicleModel(ILogger<VehicleModel> logger, IVehicleService vehicleService, IVehicleImgService vehicleImgService, IOptionalService optionalService)
        {
            _logger = logger;
            _vehicleService = vehicleService;
            _vehicleImgService = vehicleImgService;
            _optionalService = optionalService;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            await LoadOptionalAsync();
            if (id.HasValue)
            {
                var response = await _vehicleService.GetVehicleById(id.Value);

                if (response.Success)
                {
                    Vehicle = (VehicleDTO)response.Data;
                    IsEditing = true;
                    ExistingImages = Vehicle.VehicleImgs ?? new List<VehicleImgDTO>();

                    var selectedOptionIds = Vehicle.OptionalIds?.Select(id => id).ToHashSet() ?? new HashSet<int>();

                    foreach (var item in OptionalItems)
                    {
                        item.Selected = selectedOptionIds.Contains(int.Parse(item.Value));
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = response.Message ?? "Erro ao carregar veículo.";
                    return RedirectToPage("Error");
                }
            }
            else
            {
                Vehicle = new VehicleDTO();
                IsEditing = false;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (VehicleImgs.Any())
            {
                var vehicleImgsDto = new List<VehicleImgDTO>();
                foreach (var formFile in VehicleImgs)
                {
                    if (formFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await formFile.CopyToAsync(memoryStream);
                            vehicleImgsDto.Add(new VehicleImgDTO
                            {
                                ImageData = memoryStream.ToArray(),
                                FileName = formFile.FileName,
                                ContentType = formFile.ContentType
                            });
                        }
                    }
                }
                Vehicle.VehicleImgs = vehicleImgsDto;
            }

            OperationReturn result = Vehicle.Id > 0
                ? await _vehicleService.UpdateVehicle(Vehicle)
                : await _vehicleService.AddVehicle(Vehicle);

            Message = result.Message;

            if (result.Success)
            {
                TempData["SuccessMessage"] = "Veículo salvo com sucesso!";
                return RedirectToPage("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "Erro ao salvar veículo.");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostRemoveImageAsync(int imageId, int vehicleId)
        {
            if (imageId <= 0 || vehicleId <= 0)
            {
                TempData["ErrorMessage"] = "IDs inválidos.";
                return RedirectToPage("Error");
            }

            try
            {
                var result = await _vehicleImgService.DeleteImage(imageId);
                if (result.Success)
                {
                    TempData["SuccessMessage"] = "Imagem removida com sucesso.";

                    var vehicleResponse = await _vehicleService.GetVehicleById(vehicleId);
                    if (vehicleResponse.Success)
                    {
                        Vehicle = (VehicleDTO)vehicleResponse.Data;
                        ExistingImages = Vehicle.VehicleImgs ?? new List<VehicleImgDTO>();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Erro ao carregar o veículo após a exclusão da imagem.";
                        return RedirectToPage("Error");
                    }

                    return RedirectToPage(new { id = vehicleId });
                }
                else
                {
                    TempData["ErrorMessage"] = result.Message ?? "Erro ao remover a imagem.";
                    return RedirectToPage(new { id = vehicleId });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover a imagem.");
                TempData["ErrorMessage"] = "Erro ao remover a imagem.";
                return RedirectToPage(new { id = vehicleId });
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
