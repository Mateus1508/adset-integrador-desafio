using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AdSetSolution.Domain.Enums;

namespace AdSetSolution.Domain.Models
{
    public class Package
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Total { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Used { get; set; }

        [NotMapped]
        public int Available => Total - Used;

        [Required]
        public PortalType PortalType { get; set; }

        public ICollection<VehiclePackage> VehiclePackages { get; set; }
    }
}
