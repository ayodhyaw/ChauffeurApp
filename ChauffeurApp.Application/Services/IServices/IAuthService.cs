using ChauffeurApp.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace ChauffeurApp.Application.Services.IServices
{
    public interface IAuthService
    {
        Task<Result<bool>> RegisterUser(CreateUserDTO createUserDTO);
        Task<Result<LoginResponseDTO>> Login(LoginDTO loginDTO);
        Task<Result<bool>> InitiateChangePhoneNumber(ChangePhoneNumberDTO changePhoneNumberDTO);
        Task<Result<bool>> ResetPhoneNumber(ResetPhoneNumberDTO resetPhoneNumberDTO);
        Task<Result<string>> UploadFile(IFormFile _IFormFile);
        Task<Result<bool>> VerifyEmail(string userId, string token);
        Task<Result<bool>> ConfirmPhoneNumber(string userId, string phoneNumber, string token);
    }
}
