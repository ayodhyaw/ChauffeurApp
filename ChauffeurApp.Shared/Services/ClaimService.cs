using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ChauffeurApp.Shared.Services
{
    public class ClaimService : IClaimService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ClaimService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = new HttpContextAccessor();
        }
        public string GetClaim(string key)
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(key)?.Value;
        }

        public string GetUserId()
        {
            return GetClaim(ClaimTypes.NameIdentifier);
        }
    }
}
