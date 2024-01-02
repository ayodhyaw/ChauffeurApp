using ChauffeurApp.Core.Enums;

namespace ChauffeurApp.Application.DTOs
{
    public class CreateUserDTO
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; }
    }
}
