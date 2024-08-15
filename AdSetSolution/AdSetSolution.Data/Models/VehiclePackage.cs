using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdSetSolution.Domain.Models
{
    public class VehiclePackage
    {
        [Key]
        [Column(Order = 0)]
        public int VehicleId { get; set; }

        [Key]
        [Column(Order = 1)]
        public int PackageId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int PortalId { get; set; }

        [ForeignKey(nameof(VehicleId))]
        public Vehicle Vehicle { get; set; }

        [ForeignKey(nameof(PackageId))]
        public Package Package { get; set; }

        [ForeignKey(nameof(PortalId))]
        public Portal Portal { get; set; }
    }
}
