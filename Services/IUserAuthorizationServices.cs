
namespace MyShop.Services
{
    public interface IUserAuthorizationServices
    {
        Task<bool> IsAuthorized(string token, int userId);
    }
}