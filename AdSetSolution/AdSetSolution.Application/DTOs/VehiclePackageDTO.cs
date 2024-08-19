using AdSetSolution.Domain.Enums;

namespace AdSetSolution.Application.DTOs
{
    public class VehiclePackageDTO
    {
        public int VehicleId { get; set; }

        public int? PackageId { get; set; }

        public PortalType PortalType { get; set; }
    }
}
