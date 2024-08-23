using AdSetSolution.Application.DTOs;
using AdSetSolution.Application.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdSetSolution.Application.Interfaces
{
    public interface IVehicleOptionalService
    {
       Task<OperationReturn> SetVehicleOptional(IEnumerable<VehicleOptionalDTO> vehicleOptionalsDTO);
    }
}
