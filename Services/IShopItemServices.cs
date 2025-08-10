using MyShop.Models;

namespace MyShop.Services
{
    public interface IShopItemServices
    {
        Task<int> CreateAsync(CreateShopItemDto newItemDto);
        Task DeleteAsync(int id, string token);
        Task<ShopItemDto> GetByIdAsync(int id, string currencyName);
        Task<List<ShopItemDto>> GetByCategoryAsync(int categoryId, string currencyName);
        Task<List<ShopItemDto>> GetAllAsync(string currencyName);
        Task UpdateAsync(int id, CreateShopItemDto updatedShopItem);
    }
}