using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Models;

namespace MyShop.Services
{
    public class ItemCategoryServices : IItemCategoryServices
    {
        private readonly MyShopDbContext _dbContext;
        private readonly IMapper _mapper;
        public ItemCategoryServices(MyShopDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public List<ItemCategoryDto> GetAll()
        {
            List<ItemCategory> categories = _dbContext.ItemCategories.ToList();
            var categoriesDto = _mapper.Map<List<ItemCategoryDto>>(categories);
            return categoriesDto;
        }
        public ItemCategoryDto GetById(int id)
        {
            ItemCategory? category = _dbContext.ItemCategories
                .Include(category => category.ShopItems)
                .FirstOrDefault(category => category.Id == id);
            if (category == null) throw new ArgumentOutOfRangeException($"ItemCategory no. {id} does not exist.");
            ItemCategoryDto categoryDto = _mapper.Map<ItemCategoryDto>(category);
            return categoryDto;
        }
        public int Create(CreateItemCategoryDto itemCategoryDto)
        {
            ItemCategory itemCategory = _mapper.Map<ItemCategory>(itemCategoryDto);
            _dbContext.ItemCategories.Add(itemCategory);
            _dbContext.SaveChanges();
            return itemCategory.Id;
        }
        public void Update(int id, CreateItemCategoryDto itemCategoryDto)
        {
            ItemCategory? itemCategory = _dbContext.ItemCategories.Find(id);
            if (itemCategory == null) throw new ArgumentException("Item category not found");
            if (itemCategory.Id == 1) throw new ArgumentException("Category Other cannot be changed");
            itemCategory.Name = itemCategoryDto.Name;
            _dbContext.SaveChanges();
        }
        public void Delete(int id)
        {
            ItemCategory? itemCategory = _dbContext.ItemCategories.Find(id);
            if (itemCategory == null) throw new ArgumentException("Item category not found");
            if (itemCategory.Id == 1) throw new ArgumentException("Category Other cannot be removed");
            _dbContext.ItemCategories.Remove(itemCategory);
            _dbContext.SaveChanges();
        }

    }

}
