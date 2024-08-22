using System.ComponentModel.DataAnnotations;

namespace AdSetSolution.Domain.Models
{
    public class Optional
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<VehicleOptional> VehicleOptional { get; set; } = new List<VehicleOptional>();
    }
}
