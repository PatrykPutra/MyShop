using MyShop.Models;

namespace MyShop.Services
{
    public interface IOrderServices
    {
        Task AddAsync();
        Task<List<OrderDto>> GetAsync(string currencyName);
    }
}
