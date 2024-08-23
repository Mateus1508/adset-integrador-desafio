using AdSetSolution.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdSetSolution.Application.DTOs
{
    public class VehicleOptionalDTO
    {
        public int OptionalId { get; set; }
        public int VehicleId { get; set; }
    }
}
