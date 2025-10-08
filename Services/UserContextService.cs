using MyShop.Models;
using System.Security.Claims;

namespace MyShop.Services
{

    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId()
        {
            if (User == null) throw new ArgumentException("User can't be null");
            Claim? userNameIdentifierClaim = User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier);
            if (userNameIdentifierClaim == null) throw new ArgumentException("User NameIdentifier Claim not found");
            if (int.TryParse(userNameIdentifierClaim.Value,out int userId)) return userId;

            throw new ArgumentException("Unable to retrive UserId from token");
        }
    }
}
