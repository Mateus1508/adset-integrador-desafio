using System.ComponentModel.DataAnnotations;

namespace AdSetSolution.Domain.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Marca { get; set; }

        [Required]
        [MaxLength(100)]
        public string Modelo { get; set; }

        [Required]
        [Range(2000, 2024)]
        public int Ano { get; set; }

        [Required]
        [MaxLength(10)]
        public string Placa { get; set; }

        public int? Km { get; set; }

        [Required]
        [MaxLength(50)]
        public string Cor { get; set; }

        [Required]
        [Range(10000, int.MaxValue)]
        public int Preco { get; set; }

        [MaxLength(1000)]
        public string? Opcionais { get; set; }

        public virtual ICollection<VehicleImg> VehicleImgs { get; set; } = new List<VehicleImg>();

        public ICollection<VehiclePackage> VehiclePackages { get; set; }
    }
}
