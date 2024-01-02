namespace ChauffeurApp.Application.DTOs
{
    public class VehicleCreateDTO
    {
        public string Name { get; set; }
        public int SeatingCapacity { get; set; }
        public float CostPerKm { get; set; }
    }
}
