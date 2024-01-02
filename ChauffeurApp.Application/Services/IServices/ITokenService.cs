using ChauffeurApp.Application.DTOs;
using ChauffeurApp.Core.Entities;

namespace ChauffeurApp.Application.Services.IServices
{
    public interface ITokenService
    {
        Task<Result<string>> CreateToken(ApplicationUser user, IList<string> roles);
        Task<Result<TokenDTO>> RefreshAccessToken(TokenDTO tokenDTO);
        Task<Result<string>> CreateNewRefreshToken(string userId, string tokenId);
    }
}
