using MyShop.Models;

namespace MyShop.Services
{
    public interface IShoppingCartServices
    {
        Task AddItemAsync(ShoppingCartItemDto shoppingCartItemDto);
        Task<ShoppingCartDto> GetAsync(string currencyName);
        Task RemoveItemAsync(ShoppingCartItemDto shoppingCartItemDto);
        Task UpdateItemAsync(ShoppingCartItemDto shoppingCartItemDto);
    }
}
