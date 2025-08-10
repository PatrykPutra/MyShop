using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Exceptions;
using MyShop.Models;
using System.Security.Authentication;

namespace MyShop.Services
{
    public class ItemCategoryServices : IItemCategoryServices
    {
        private readonly MyShopDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUserServices _userServices;
        public ItemCategoryServices(MyShopDbContext dbContext, IMapper mapper, IUserServices userServices)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userServices = userServices;
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
            
            User user = await _userServices.GetAsync(itemCategoryDto.Token);
            if (user.IsAdmin == false) throw new UnauthorizedRequestException("Unauthorized request");
           
                   
            ItemCategory itemCategory = _mapper.Map<ItemCategory>(itemCategoryDto);
            _dbContext.ItemCategories.Add(itemCategory);
            await _dbContext.SaveChangesAsync();
            return itemCategory.Id;
        }
        public async Task UpdateAsync(int id, CreateItemCategoryDto itemCategoryDto)
        {
          
            User user = await _userServices.GetAsync(itemCategoryDto.Token);
            if (user.IsAdmin == false) throw new UnauthorizedRequestException("Unauthorized request");
          
            ItemCategory? itemCategory = _dbContext.ItemCategories.Find(id);
            if (itemCategory == null) throw new NotFoundException("Item category not found");
            if (itemCategory.Id == 1) throw new ArgumentException("Category Other cannot be changed");
            itemCategory.Name = itemCategoryDto.Name;
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id,string token)
        {
            
            User user = await _userServices.GetAsync(token);
            if (user.IsAdmin == false) throw new UnauthorizedRequestException("Unauthorized request");
            
            ItemCategory? itemCategory = _dbContext.ItemCategories.Find(id);
            if (itemCategory == null) throw new NotFoundException("Item category not found");
            if (itemCategory.Id == 1) throw new ArgumentException("Category Other cannot be removed");
            _dbContext.ItemCategories.Remove(itemCategory);
            await _dbContext.SaveChangesAsync();
        }

    }

}
