using System.ComponentModel.DataAnnotations;

namespace ChauffeurApp.Core.Common
{
    public abstract class BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
