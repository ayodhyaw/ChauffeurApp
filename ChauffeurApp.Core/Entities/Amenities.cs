using ChauffeurApp.Core.Common;

namespace ChauffeurApp.Core.Entities
{
    public class Amenities : BaseEntity
    {
        public string Type { get; set; }
        public float Price { get; set; }

        //Relationships
        public ICollection<VehicleAmenities>? VehicleAmenities { get; set; } 
    }
}
