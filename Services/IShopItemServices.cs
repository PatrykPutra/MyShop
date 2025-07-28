using MyShop.Models;

namespace MyShop.Services
{
    public interface IShopItemServices
    {
        Task<int> CreateAsync(CreateShopItemDto newItemDto);
        Task DeleteAsync(int id);
        Task<ShopItemDto> GetByIdAsync(int id, string currencyName);
        Task UpdateAsync(int id, CreateShopItemDto updatedShopItem);
    }
}