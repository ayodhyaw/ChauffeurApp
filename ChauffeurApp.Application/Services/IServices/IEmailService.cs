using ChauffeurApp.Application.DTOs;

namespace ChauffeurApp.Application.Services.IServices
{
    public interface IEmailService
    {
        void SendEmail(EmailDTO request);
        Task SendChangePhoneNumberTokenAsync(string emailAddress, string token);
    }
}
