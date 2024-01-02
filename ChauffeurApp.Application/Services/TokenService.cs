using ChauffeurApp.Application.DTOs;
using ChauffeurApp.Application.Services.IServices;
using ChauffeurApp.Core.Entities;
using ChauffeurApp.DataAccess.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChauffeurApp.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenService(IConfiguration config, AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _context = context;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            _userManager = userManager;
        }
        public async Task<Result<string>> CreateNewRefreshToken(string userId, string tokenId)
        {
            RefreshToken refreshToken = new()
            {
                IsValid = true,
                UserId = userId,
                JwtTokenId = tokenId,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                Refresh_Token = Guid.NewGuid() + "-" + Guid.NewGuid(),
            };
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
            return Result<string>.Success(refreshToken.Refresh_Token);
        }

        public async Task<Result<string>> CreateToken(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FullName),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var duration = _config["Jwt:DurationMinutes"];
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(duration)),
                SigningCredentials = creds,
                Issuer = _config["Jwt:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenResult = tokenHandler.WriteToken(token);

            if (tokenResult is null)
            {
                return Result<string>.Failure("Token is null");
            }

            return Result<string>.Success(tokenResult);

        }

        public async Task<Result<TokenDTO>> RefreshAccessToken(TokenDTO tokenDTO)
        {
            try
            {
                var existingRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(u => u.Refresh_Token == tokenDTO.RefreshToken);

                if (existingRefreshToken != null && IsRefreshTokenValid(tokenDTO.RefreshToken, existingRefreshToken.UserId, existingRefreshToken.JwtTokenId))
                {
                    var accessTokenData = GetAccessTokenData(tokenDTO.Token);

                    if (accessTokenData.isSuccessful)
                    {
                        existingRefreshToken.IsValid = false;
                        await _context.SaveChangesAsync();

                        var user = await _userManager.FindByIdAsync(accessTokenData.userId);
                        var roles = await _userManager.GetRolesAsync(user);
                        var jwtTokenId = $"JTI{Guid.NewGuid()}";
                        var newAccessToken = await CreateToken(user, roles);
                        var newRefreshToken = await CreateNewRefreshToken(user.Id.ToString(), jwtTokenId);


                        TokenDTO newToken = new TokenDTO
                        {
                            Token = newAccessToken.Value,
                            RefreshToken = newRefreshToken.Value
                        };

                        return Result<TokenDTO>.Success(newToken);
                    }
                }

                return Result<TokenDTO>.Failure("Invalid refresh token or access token");
            }
            catch (Exception ex)
            {
                throw new Exception("Error refreshing access token", ex);
            }
        }
        private bool IsRefreshTokenValid(string refreshToken, string userId, string jwtTokenId)
        {
            RefreshToken storedRefreshToken = GetRefreshTokenFromDatabase(userId, jwtTokenId);

            return storedRefreshToken != null &&
                   storedRefreshToken.Refresh_Token == refreshToken &&
                   storedRefreshToken.IsValid &&
                   DateTime.UtcNow <= storedRefreshToken.ExpiresAt;
        }

        private RefreshToken GetRefreshTokenFromDatabase(string userId, string jwtTokenId)
        {
            return _context.RefreshTokens.FirstOrDefault(rt => rt.UserId == userId && rt.JwtTokenId == jwtTokenId);
        }
        private (bool isSuccessful, string userId, string tokenId) GetAccessTokenData(string accessToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _key,
                    ValidateIssuer = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidateAudience = false
                };

                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(accessToken, validationParameters, out validatedToken);

                var jwtTokenId = (validatedToken as JwtSecurityToken)?.Claims?.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Jti)?.Value;
                var userId = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return (true, userId, jwtTokenId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAccessTokenData: {ex.Message}");
                return (false, null, null);
            }
        }

    }
}
