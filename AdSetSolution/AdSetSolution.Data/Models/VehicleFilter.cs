namespace AdSetSolution.Domain.Models
{
    public class VehicleFilter
    {
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int? AnoMin { get; set; }
        public int? AnoMax { get; set; }
        public string Placa { get; set; }
        public string? Fotos { get; set; }
        public string Cor { get; set; }
        public string? Preco { get; set; }
        public string Opcionais { get; set; }
    }
}
