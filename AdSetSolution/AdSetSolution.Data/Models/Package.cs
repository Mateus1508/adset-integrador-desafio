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
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0, int.MaxValue)]
        public int Total { get; set; }

        [Required]
        public PortalType PortalType { get; set; }

        [NotMapped]
        public int Used => VehiclePackages.Count;

        [NotMapped]
        public int Available => Total - Used;

        [NotMapped]
        public bool IsExhausted => Total == Used;

        [Required]

        public virtual ICollection<VehiclePackage> VehiclePackages { get; set; } = new List<VehiclePackage>();
    }
}
