
namespace MyShop.Services
{
    public interface ILogoutServices
    {
        Task<string> GenerateJwt();
    }
}