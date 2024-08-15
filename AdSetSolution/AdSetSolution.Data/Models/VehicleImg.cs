using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AdSetSolution.Domain.Models
{
    public class VehicleImg
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Vehicle))]
        public int VehicleId { get; set; }

        public byte[] ImageData { get; set; }

        [MaxLength(255)]
        public string? FileName { get; set; }

        [MaxLength(50)]
        public string? ContentType { get; set; }

        public virtual Vehicle Vehicle { get; set; }
    }
}
