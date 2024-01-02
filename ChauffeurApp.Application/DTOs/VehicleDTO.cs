namespace ChauffeurApp.Application.DTOs
{
    public class VehicleDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int SeatingCapacity { get; set; }
        public float CostPerKm { get; set; }
        public bool AvailabilityStatus { get; set; }
    }
}
