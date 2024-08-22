using AdSetSolution.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AdSetSolution.Application.DTOs
{
    public class VehicleDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A marca é obrigatória.")]
        [StringLength(100, ErrorMessage = "A marca não pode exceder 100 caracteres.")]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "O modelo é obrigatório.")]
        [StringLength(100, ErrorMessage = "O modelo não pode exceder 100 caracteres.")]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ano é obrigatório.")]
        [Range(2000, 2024, ErrorMessage = "O ano deve estar entre 2000 e 2024.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "A placa é obrigatória.")]
        [StringLength(10, ErrorMessage = "A placa não pode exceder 10 caracteres.")]
        public string LicensePlate { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "A quilometragem deve ser um valor positivo.")]
        public int? Mileage { get; set; }

        [Required(ErrorMessage = "A cor é obrigatória.")]
        [StringLength(50, ErrorMessage = "A cor não pode exceder 50 caracteres.")]
        public string Color { get; set; } = string.Empty;

        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Range(10000, int.MaxValue, ErrorMessage = "O preço deve ser maior que 10.000.")]
        public int Price { get; set; }

        public ICollection<VehicleOptionalDTO>? VehicleOptionals { get; set; }

        [MaxLength(15, ErrorMessage = "Não é permitido adicionar mais de 15 fotos ao veículo.")]
        public ICollection<VehicleImgDTO>? VehicleImgs { get; set; } = new List<VehicleImgDTO>();

        public ICollection<VehiclePackageDTO>? VehiclePackages { get; set; }
    }
}
