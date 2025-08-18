using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Exceptions;
using MyShop.Models;
using System.Security.Authentication;

namespace MyShop.Services
{
    public interface IShopItemServices
    {
        Task<int> CreateAsync(CreateShopItemDto newItemDto);
        Task DeleteAsync(int id);
        Task<ShopItemDto> GetByIdAsync(int id, string currencyName);
        Task<List<ShopItemDto>> GetByCategoryAsync(int categoryId, string currencyName);
        Task<List<ShopItemDto>> GetAllAsync(string currencyName);
        Task UpdateAsync(int id, CreateShopItemDto updatedShopItem);
    }
    public class ShopItemServices : IShopItemServices
    {
        private readonly MyShopDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IExchangeRatesServices _exchangeRatesServices;
        private readonly IUserContextService _userContextService;
        private readonly ILogger<ShopItemServices> _logger;
        public ShopItemServices(MyShopDbContext dbContext, IMapper mapper, IExchangeRatesServices exchangeRatesServices, IUserContextService userContextServices, ILogger<ShopItemServices> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _exchangeRatesServices = exchangeRatesServices;
            _userContextService = userContextServices;
            _logger = logger;
        }
        public async Task<int> CreateAsync(CreateShopItemDto newItemDto)
        {
            int userId = _userContextService.GetUserId();
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
            _logger.LogInformation($"Shop item No: {shopItem.Id} {shopItem.Name} created by user {userId}");
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
            int userId = _userContextService.GetUserId();

            ShopItem? existingShopItem = await _dbContext.ShopItems.FindAsync(id);
            if (existingShopItem == null) throw new NotFoundException($"ShopItem with Id: {id} does not exist.");

            ItemCategory? category = await _dbContext.ItemCategories.FindAsync(updatedShopItem.CategoryId);
            if (category == null) throw new ArgumentException($"ItemCategory with Id: {updatedShopItem.CategoryId} does not exist.");

            _logger.LogInformation($"Shop item No: {existingShopItem.Id} {existingShopItem.Name} updated by user {userId}");

            existingShopItem.Name = updatedShopItem.Name;
            existingShopItem.Text = updatedShopItem.Text;
            existingShopItem.PriceUSD = updatedShopItem.PriceUSD;
            existingShopItem.CategoryId = updatedShopItem.CategoryId;
            existingShopItem.Quantity = updatedShopItem.Quantity;

            await _dbContext.SaveChangesAsync();
            
        }
        public async Task DeleteAsync(int id)
        {
            int userId = _userContextService.GetUserId();

            ShopItem? shopItem = await _dbContext.ShopItems.FindAsync(id);
            if (shopItem == null) throw new NotFoundException($"ShopItem with Id: {id} does not exist.");

            _logger.LogInformation($"Shop item No: {shopItem.Id} {shopItem.Name} deleted by user {userId}");

            _dbContext.ShopItems.Remove(shopItem);
            await _dbContext.SaveChangesAsync();
        }
    }
}
