using MyShop.Models;

namespace MyShop.Services
{
    public interface IShopItemServices // Interfejs powinien być w oddzielnym pliku
    {
        Task<int> CreateAsync(CreateShopItemDto newItemDto);
        Task DeleteAsync(int id);
        Task<ShopItemDto> GetByIdAsync(int id, string currencyName);
        Task<List<ShopItemDto>> GetAllAsync(int? categoryId, string? currencyName);
        Task UpdateAsync(int id, CreateShopItemDto updatedShopItem);
    }
}
