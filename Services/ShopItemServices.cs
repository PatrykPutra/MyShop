using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Models;

namespace MyShop.Services
{
    public class ShopItemServices : IShopItemServices
    {
        private readonly MyShopDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IExchangeRatesServices _exchangeRatesServices;
        public ShopItemServices(MyShopDbContext dbContext, IMapper mapper, IExchangeRatesServices exchangeRatesServices)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _exchangeRatesServices = exchangeRatesServices;
        }
        public async Task<int> CreateAsync(CreateShopItemDto newItemDto)
        {
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
            if (shopItem == null) throw new ArgumentOutOfRangeException($"ShopItem with Id = {id} does not exist.");
            try
            {
                exchangeRate = await _exchangeRatesServices.GetExchangeRateAsync(currencyName);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Can not find exchangeRate parameter", ex);
            }

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
        public async Task UpdateAsync(int id, CreateShopItemDto updatedShopItem)
        {
            ShopItem? existingShopItem = await _dbContext.ShopItems.FindAsync(id);
            if (existingShopItem == null) throw new ArgumentOutOfRangeException($"ShopItem with Id: {id} does not exist.");
            ItemCategory? category = await _dbContext.ItemCategories.FindAsync(updatedShopItem.CategoryId);
            if (category == null) throw new ArgumentException($"ItemCategory with Id: {updatedShopItem.CategoryId} does not exist.");

            existingShopItem.Name = updatedShopItem.Name;
            existingShopItem.Text = updatedShopItem.Text;
            existingShopItem.PriceUSD = updatedShopItem.PriceUSD;
            existingShopItem.CategoryId = updatedShopItem.CategoryId;
            existingShopItem.Quantity = updatedShopItem.Quantity;

            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            ShopItem? shopItem = await _dbContext.ShopItems.FindAsync(id);
            if (shopItem == null) throw new ArgumentOutOfRangeException($"ShopItem with Id: {id} does not exist.");
            _dbContext.ShopItems.Remove(shopItem);
            await _dbContext.SaveChangesAsync();
        }
    }
}
