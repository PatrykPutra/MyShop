
using MyShop.Models;

namespace MyShop.Services
{
    public interface IShoppingCartServices
    {
        Task AddItemAsync(ShoppingCartItemDto shoppingCartItemDto);
        Task RemoveItemAsync(ShoppingCartItemDto shoppingCartItemDto);
    }
}