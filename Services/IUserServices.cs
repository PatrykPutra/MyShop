using MyShop.Models;

namespace MyShop.Services
{
    public interface IUserServices
    {
        Task CreateAsync(CreateUserDto userDto);
        Task<User> GetAsync(string token);
    }
}