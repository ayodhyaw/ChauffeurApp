namespace ChauffeurApp.Application.DTOs
{
    public class VehicleAmenitiesDTO
    {
        public long VehicleId { get; set; }
        public List<long> AmenityIds { get; set; }
    }
}
