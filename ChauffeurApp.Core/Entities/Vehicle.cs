using ChauffeurApp.Core.Common;

namespace ChauffeurApp.Core.Entities
{
    public class Vehicle : BaseEntity, IAuditedEntity
    {
        public string Name { get; set; }
        public int SeatingCapacity { get; set; }
        public float CostPerKm { get; set; }
        public bool AvailabilityStatus { get; set; } = true;
        public string? VehicleImagePath { get; set; }
        public DateTime? CreatedAt { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? UpdatedById { get; set; }
        public DateTime? DeletedAt { get; set; }
        public long? DeletedById { get; set; }

        //Relationships
        public long? BrandID { get; set; }
        public Brand? Brand { get; set; }
        public long? TypeID { get; set; }
        public VehicleType? VehicleType { get; set;}
        public ICollection<VehicleAmenities>? VehicleAmenities { get; set; }
        public ICollection<VehicleImages>? vehicleImages { get; set; }
    }
}
