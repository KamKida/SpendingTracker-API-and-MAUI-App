using Microsoft.AspNetCore.Http;
using SpendingTracker.Application.Interfaces.ContextServices;
using System.Security.Claims;

namespace SpendingTracker.Application.ContextServices
{
    public class UserContextService : IUserContextService
    {
        private IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

        public Guid? GetUserId() =>
            
            User is null ? null : Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    }
}
