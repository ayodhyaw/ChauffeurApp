using ChauffeurApp.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace ChauffeurApp.Core.Entities
{
    public class ApplicationUser : IdentityUser<long>
    {
        public string FullName { get; set; }
        public Gender Gender { get; set; }
        public ActiveStatus ActiveStatus { get; set; } = ActiveStatus.Active;
        public string? Address { get; set; }
        public string? ProfilePicturePath { get; set; }
    }
}
