using MyShop.Models;

namespace MyShop.Services
{
    
    public interface IOrderServices
    {
        Task AddAsync(CreateOrderDto newOrderDto);
    }
}