using System.ComponentModel.DataAnnotations;

namespace AdSetSolution.Domain.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Brand { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Model { get; set; } = string.Empty;

        [Required]
        [Range(2000, 2024)]
        public int Year { get; set; }

        [Required]
        [MaxLength(10)]
        public string LicensePlate { get; set; } = string.Empty;

        public int? Mileage { get; set; }

        [Required]
        [MaxLength(50)]
        public string Color { get; set; } = string.Empty;

        [Required]
        [Range(10000, int.MaxValue)]
        public int Price { get; set; }

        public virtual ICollection<VehicleImg> VehicleImgs { get; set; } = new List<VehicleImg>();

        public virtual ICollection<VehiclePackage> VehiclePackages { get; set; } = new List<VehiclePackage>();

        public virtual ICollection<VehicleOptional> VehicleOptional { get; set; } = new List<VehicleOptional>();
    }
}
