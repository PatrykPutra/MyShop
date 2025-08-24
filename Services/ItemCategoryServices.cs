using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Entities;
using MyShop.Exceptions;
using MyShop.Models;
using System.Security.Authentication;

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
    public class ItemCategoryServices : IItemCategoryServices
    {
        private readonly MyShopDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ILogger _logger;
        public ItemCategoryServices(MyShopDbContext dbContext, IMapper mapper, IUserContextService userContextService, ILogger<ItemCategoryServices> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userContextService = userContextService;
            _logger = logger;
        }
        public async Task<List<ItemCategoryDto>> GetAllAsync()
        {
            List<ItemCategory> categories = await _dbContext.ItemCategories.ToListAsync();
            var categoriesDto = _mapper.Map<List<ItemCategoryDto>>(categories);
            return categoriesDto;
        }
        public async Task<ItemCategoryDto> GetByIdAsync(int id)
        {
            ItemCategory? category = await _dbContext.ItemCategories
                .Include(category => category.ShopItems)
                .FirstOrDefaultAsync(category => category.Id == id);
            if (category == null) throw new ArgumentException($"ItemCategory no. {id} does not exist.");
            ItemCategoryDto categoryDto = _mapper.Map<ItemCategoryDto>(category);
            return categoryDto;
        }
        public async Task<int> CreateAsync(CreateItemCategoryDto itemCategoryDto)
        {
            int userId = _userContextService.GetUserId();
            ItemCategory itemCategory = _mapper.Map<ItemCategory>(itemCategoryDto);
            _dbContext.ItemCategories.Add(itemCategory);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Item category {itemCategory.Id} {itemCategoryDto.Name} created by user: {userId}");
            return itemCategory.Id;
        }
        public async Task UpdateAsync(int id, CreateItemCategoryDto itemCategoryDto)
        {
            int userId = _userContextService.GetUserId();
            ItemCategory? itemCategory = _dbContext.ItemCategories.Find(id);
            if (itemCategory == null) throw new NotFoundException("Item category not found");
            if (itemCategory.Id == 1) throw new ArgumentException("Category Other cannot be changed");
            _logger.LogInformation($"Item category No: {itemCategory.Id} {itemCategory.Name} changed to {itemCategoryDto.Name} by user: {userId}");
            itemCategory.Name = itemCategoryDto.Name;
            
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            int userId = _userContextService.GetUserId();
            ItemCategory? itemCategory = _dbContext.ItemCategories.Find(id);
            if (itemCategory == null) throw new NotFoundException("Item category not found");
            if (itemCategory.Id == 1) throw new ArgumentException("Category Other cannot be removed");
            _logger.LogInformation($"Item category {id} {itemCategory.Name} deleted by user: {userId}");
            _dbContext.ItemCategories.Remove(itemCategory);
            await _dbContext.SaveChangesAsync();
        }

    }

}
