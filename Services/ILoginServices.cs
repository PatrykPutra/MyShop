using MyShop.Models;


namespace MyShop.Services
{
    public interface ILoginServices
    {
        Task<string> GenerateJwt(CredentialsDto credentials);
        
    }
}
