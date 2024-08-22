namespace AdSetSolution.Domain.Models
{
    public class VehicleFilter
    {
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int? YearMin { get; set; }
        public int? YearMax { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public string? Photos { get; set; }
        public string Color { get; set; } = string.Empty;
        public string? Price { get; set; }
        public ICollection<int> VehicleOptionalIds { get; set; } = new List<int>();
    }
}
