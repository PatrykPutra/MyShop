using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Exceptions;
using MyShop.Models;
using System.Security.Authentication;

namespace MyShop.Services
{
    public class ShopItemServices : IShopItemServices
    {
        private readonly MyShopDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IExchangeRatesServices _exchangeRatesServices;
        private readonly IUserServices _userServices;
        public ShopItemServices(MyShopDbContext dbContext, IMapper mapper, IExchangeRatesServices exchangeRatesServices, IUserServices userServices)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _exchangeRatesServices = exchangeRatesServices;
            _userServices = userServices;
        }
        public async Task<int> CreateAsync(CreateShopItemDto newItemDto)
        {
            User user = await _userServices.GetAsync(newItemDto.Token);
            if (user.IsAdmin == false) throw new UnauthorizedRequestException("Unauthorized request.");
            

            ItemCategory? category = await _dbContext.ItemCategories.FindAsync(newItemDto.CategoryId);
            if (category == null) throw new ArgumentException($"Category no. {newItemDto.CategoryId} does not exist.");

            ShopItem shopItem = new ShopItem
            {
                Name = newItemDto.Name,
                Text = newItemDto.Text,
                PriceUSD = newItemDto.PriceUSD,
                Quantity = newItemDto.Quantity,
                Category = category,
                CategoryId = newItemDto.CategoryId
            };

            _dbContext.ShopItems.Add(shopItem);
            await _dbContext.SaveChangesAsync();
            return shopItem.Id;

        }
        public async Task<ShopItemDto> GetByIdAsync(int id, string currencyName)
        {
            ShopItem? shopItem = _dbContext.ShopItems.Include(shopItem => shopItem.Category).FirstOrDefault(shopItem => shopItem.Id == id);
            decimal exchangeRate;
            if (shopItem == null) throw new NotFoundException($"ShopItem with Id = {id} does not exist.");
            
            exchangeRate = await _exchangeRatesServices.GetExchangeRateAsync(currencyName);
           
            ShopItemDto shopItemDto = new()
            {
                Name = shopItem.Name,
                Text = shopItem.Text,
                Price = shopItem.PriceUSD * exchangeRate,
                PriceCurrency = currencyName,
                CategoryId = shopItem.CategoryId,
                Quantity = shopItem.Quantity,
            };
            return shopItemDto;
        }
        public async Task<List<ShopItemDto>> GetByCategoryAsync(int categoryId,string currencyName)
        {
            decimal exchangeRate;
         
            exchangeRate = await _exchangeRatesServices.GetExchangeRateAsync(currencyName);
           
            List<ShopItem> shopItems = await _dbContext.ShopItems.Include(shopItem => shopItem.Category).Where(shopItem => shopItem.CategoryId == categoryId).ToListAsync();
            List<ShopItemDto> shopItemsDtos = shopItems.Select(shopItem => new ShopItemDto()
            {
                Name = shopItem.Name,
                Text = shopItem.Text,
                Price = shopItem.PriceUSD * exchangeRate,
                PriceCurrency = currencyName,
                CategoryId = shopItem.CategoryId,
                Quantity = shopItem.Quantity,
            }).ToList();
            return shopItemsDtos;

        }
        public async Task<List<ShopItemDto>> GetAllAsync(string currencyName)
        {
            decimal exchangeRate;
            
            exchangeRate = await _exchangeRatesServices.GetExchangeRateAsync(currencyName);
          
            List<ShopItem> shopItems = await _dbContext.ShopItems.Include(shopItem => shopItem.Category).Where(shopItem=>shopItem!=null).ToListAsync();
            List<ShopItemDto> shopItemsDtos = shopItems.Select(shopItem => new ShopItemDto()
            {
                Name = shopItem.Name,
                Text = shopItem.Text,
                Price = shopItem.PriceUSD * exchangeRate,
                PriceCurrency = currencyName,
                CategoryId = shopItem.CategoryId,
                Quantity = shopItem.Quantity,
            }).ToList();
            return shopItemsDtos;
        }

        public async Task UpdateAsync(int id, CreateShopItemDto updatedShopItem)
        {
           
            User user = await _userServices.GetAsync(updatedShopItem.Token);
            if (user.IsAdmin == false) throw new UnauthorizedRequestException("Unauthorized request.");
         
            ShopItem? existingShopItem = await _dbContext.ShopItems.FindAsync(id);
            if (existingShopItem == null) throw new NotFoundException($"ShopItem with Id: {id} does not exist.");
            ItemCategory? category = await _dbContext.ItemCategories.FindAsync(updatedShopItem.CategoryId);
            if (category == null) throw new ArgumentException($"ItemCategory with Id: {updatedShopItem.CategoryId} does not exist.");

            existingShopItem.Name = updatedShopItem.Name;
            existingShopItem.Text = updatedShopItem.Text;
            existingShopItem.PriceUSD = updatedShopItem.PriceUSD;
            existingShopItem.CategoryId = updatedShopItem.CategoryId;
            existingShopItem.Quantity = updatedShopItem.Quantity;

            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id,string token)
        {
           
            User user = await _userServices.GetAsync(token);
            if (user.IsAdmin == false) throw new UnauthorizedRequestException("Unauthorized request.");
            
            ShopItem? shopItem = await _dbContext.ShopItems.FindAsync(id);
            if (shopItem == null) throw new NotFoundException($"ShopItem with Id: {id} does not exist.");
            _dbContext.ShopItems.Remove(shopItem);
            await _dbContext.SaveChangesAsync();
        }
    }
}
