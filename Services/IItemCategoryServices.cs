using MyShop.Models;

namespace MyShop.Services
{
    public interface IItemCategoryServices
    {
        int Create(CreateItemCategoryDto itemCategoryDto);
        void Delete(int id);
        List<ItemCategoryDto> GetAll();
        ItemCategoryDto GetById(int id);
        void Update(int id, CreateItemCategoryDto itemCategoryDto);
    }
}