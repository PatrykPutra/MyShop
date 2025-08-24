using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Entities;
using MyShop.Models;
using System.Security.Authentication;

namespace MyShop.Services
{
    public interface IShoppingCartServices
    {
        Task AddItemAsync(ShoppingCartItemDto shoppingCartItemDto);
        Task<ShoppingCartDto> GetAsync(string currencyName);
        Task RemoveItemAsync(ShoppingCartItemDto shoppingCartItemDto);
    }
    public class ShoppingCartServices : IShoppingCartServices
    {
        private readonly MyShopDbContext _dbContext;
        private readonly IUserServices _userServices;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;
        private readonly IExchangeRatesServices _exchangeRatesServices;
        public ShoppingCartServices(MyShopDbContext dbContext, IUserServices userServices, IUserContextService userContextService,IMapper mapper,IExchangeRatesServices exchangeRatesServices)
        {
            _dbContext = dbContext;
            _userServices = userServices;
            _userContextService = userContextService;
            _mapper = mapper;
            _exchangeRatesServices = exchangeRatesServices;
        }

     
        public async Task AddItemAsync(ShoppingCartItemDto shoppingCartItemDto)
        {
            int userId = _userContextService.GetUserId();
            User user = await _userServices.GetAsync(userId);
            for (int i = 0; i < shoppingCartItemDto.Quantity; i++) user.Cart.ShopItemsIds.Add(shoppingCartItemDto.ItemId);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<ShoppingCartDto> GetAsync(string? currencyName)
        {
            int userId = _userContextService.GetUserId();
            User user = await _userServices.GetAsync(userId);
            
            decimal exchangeRate = currencyName == null? 1.00M : await _exchangeRatesServices.GetExchangeRateAsync(currencyName);
            if (currencyName == null) currencyName = "USD";

            var itemIds = user.Cart.ShopItemsIds;
            var shopItemDtos = new List<ShopItemDto>();
            foreach (var itemId in itemIds)
            {
                var shopItem = await _dbContext.ShopItems.FindAsync(itemId);
                var shopItemDto = _mapper.Map<ShopItemDto>(shopItem);
                shopItemDto.Quantity = 1;
                shopItemDto.Price = shopItemDto.Price* exchangeRate;
                shopItemDto.PriceCurrency = currencyName;

                if(shopItemDtos.Any(shopItemDto=>shopItemDto.Id == itemId))
                {
                    var item = shopItemDtos.First(shopItemDto=>shopItemDto.Id==itemId);
                    item.Price += shopItemDto.Price;
                    item.Quantity++;
                }
                    
                else shopItemDtos.Add(shopItemDto);
            }
            var shoppingCartDto = new ShoppingCartDto()
            {
                Id = user.CartId,
                ShopItems = shopItemDtos,
            };
            return shoppingCartDto; 
        }

        public async Task RemoveItemAsync(ShoppingCartItemDto shoppingCartItemDto)
        {
            int userId = _userContextService.GetUserId();
            User user = await _userServices.GetAsync(userId);
            for (int i = 0; i < shoppingCartItemDto.Quantity; i++) user.Cart.ShopItemsIds.Remove(shoppingCartItemDto.ItemId);
            await _dbContext.SaveChangesAsync();
        }   
   
    }
}
