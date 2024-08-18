using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdSetSolution.Domain.Enums;

namespace AdSetSolution.Domain.Models
{
    public class VehiclePackage
    {
        [Column(Order = 0)]
        public int VehicleId { get; set; }

        [Column(Order = 1)]
        public int PackageId { get; set; }

        [Column(Order = 2)]
        public PortalType PortalType { get; set; }

        [ForeignKey(nameof(VehicleId))]
        public Vehicle Vehicle { get; set; }

        [ForeignKey(nameof(PackageId))]
        public Package Package { get; set; }
    }
}
