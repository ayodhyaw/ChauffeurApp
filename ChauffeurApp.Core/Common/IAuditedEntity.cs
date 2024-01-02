namespace ChauffeurApp.Core.Common
{
    public interface IAuditedEntity
    {
        public DateTime? CreatedAt { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? UpdatedById { get; set; }
        public DateTime? DeletedAt { get; set; }
        public long? DeletedById { get; set; }
    }
}
