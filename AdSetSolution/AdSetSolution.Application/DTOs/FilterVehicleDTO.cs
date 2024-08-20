using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdSetSolution.Application.DTOs
{
    public class FilterVehicleDTO
    {
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int? Ano { get; set; }
        public string Placa { get; set; }
        public int? Km { get; set; }
        public string Cor { get; set; }
        public decimal? Preco { get; set; }
        public string Opcionais { get; set; }
    }
}
