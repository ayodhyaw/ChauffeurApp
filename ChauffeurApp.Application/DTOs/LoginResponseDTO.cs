namespace ChauffeurApp.Application.DTOs
{
    public class LoginResponseDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
