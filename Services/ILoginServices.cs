using MyShop.Models;

namespace MyShop.Services
{
    public interface ILoginServices
    {
        Task<string> Login(CredentialsDto credentials);
    }
}