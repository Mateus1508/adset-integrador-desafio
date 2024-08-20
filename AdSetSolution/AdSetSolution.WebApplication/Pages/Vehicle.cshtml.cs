using AdSetSolution.Application.DTOs;
using AdSetSolution.Application.Interfaces;
using AdSetSolution.Application.Services;
using AdSetSolution.Application.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdSetSolution.WebApplication.Pages
{
    public class VehicleModel : PageModel
    {
        private readonly ILogger<VehicleModel> _logger;
        private readonly IVehicleService _vehicleService;
        private readonly IVehicleImgService _vehicleImgService;

        [BindProperty]
        public VehicleDTO Vehicle { get; set; }

        [BindProperty]
        public List<IFormFile> VehicleImgs { get; set; } = new List<IFormFile>();

        public bool IsEditing { get; set; }

        public IEnumerable<VehicleImgDTO> ExistingImages { get; set; } = new List<VehicleImgDTO>();

        public string Message { get; set; }

        public VehicleModel(ILogger<VehicleModel> logger, IVehicleService vehicleService, IVehicleImgService vehicleImgService)
        {
            _logger = logger;
            _vehicleService = vehicleService;
            _vehicleImgService = vehicleImgService;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id.HasValue)
            {
                var response = await _vehicleService.GetVehicleById(id.Value);

                if (response.Success)
                {
                    Vehicle = (VehicleDTO)response.Data;
                    IsEditing = true;
                    ExistingImages = Vehicle.VehicleImgs ?? [];
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

            OperationReturn result;

            if (Vehicle.Id > 0)
            {
                result = await _vehicleService.UpdateVehicle(Vehicle);
            }
            else
            {
                result = await _vehicleService.AddVehicle(Vehicle);
            }

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

        [HttpPost]
        public async Task<IActionResult> OnPostRemoveImageAsync(int imageId, int vehicleId)
        {
            if (imageId <= 0 || vehicleId <= 0)
                return BadRequest("ID inválido.");

            try
            {
                var result = await _vehicleImgService.DeleteImage(imageId);
                if (result.Success)
                {
                    TempData["SuccessMessage"] = "Imagem removida com sucesso.";
                    return RedirectToPage(new { id = vehicleId });
                }

                TempData["ErrorMessage"] = "Erro ao remover a imagem.";
                return RedirectToPage(new { id = vehicleId });
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Erro ao remover a imagem.";
                return RedirectToPage(new { id = vehicleId });
            }
        }
    }
}
