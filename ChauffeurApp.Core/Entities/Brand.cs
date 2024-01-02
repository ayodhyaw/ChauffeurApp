using ChauffeurApp.Core.Common;

namespace ChauffeurApp.Core.Entities
{
    public class Brand : BaseEntity, IAuditedEntity
    {
        //[Required]
        public string Name { get; set; }
        public string Features { get; set; }
        public DateTime? CreatedAt { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? UpdatedById { get; set; }
        public DateTime? DeletedAt { get; set; }
        public long? DeletedById { get; set; }

        //Relationships
        public ICollection<Vehicle>? Vehicles { get; set; }
    }
}
