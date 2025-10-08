using MyShop.Entities;
using MyShop.Models;

namespace MyShop.Services
{
    public interface IUserServices
    {
        Task CreateAsync(CreateUserDto userDto);
        Task<User> GetAsync(int userId);
    }
}
