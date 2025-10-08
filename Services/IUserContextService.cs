using System.Security.Claims;

namespace MyShop.Services
{
    public interface IUserContextService
    {
        ClaimsPrincipal? User { get; }

        int GetUserId();
    }
}
