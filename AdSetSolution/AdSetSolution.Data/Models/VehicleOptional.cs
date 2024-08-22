using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdSetSolution.Domain.Models
{
    public class VehicleOptional
    {
        [Key, Column(Order = 0)]
        [ForeignKey(nameof(Optional))]
        public int OptionalId { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey(nameof(Vehicle))]
        public int VehicleId { get; set; }

        public virtual Optional Optional { get; set; }
        public virtual Vehicle Vehicle { get; set; }

    }
}
