using MyShop.Models;

namespace MyShop.Services
{
    public interface IItemCategoryServices
    {
        Task<int> CreateAsync(CreateItemCategoryDto itemCategoryDto);
        Task DeleteAsync(int id);
        Task<List<ItemCategoryDto>> GetAllAsync();
        Task<ItemCategoryDto> GetByIdAsync(int id);
        Task UpdateAsync(int id, CreateItemCategoryDto itemCategoryDto);
    }

}
