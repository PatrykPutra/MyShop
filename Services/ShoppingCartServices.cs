using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Entities;
using MyShop.Exceptions;
using MyShop.Models;
using System.Security.Authentication;

namespace MyShop.Services
{
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
            
            if (!_dbContext.ShopItems.Any(shopItem => shopItem.Id == shoppingCartItemDto.ItemId)) 
                throw new NotFoundException($"Could not find shop item No. {shoppingCartItemDto.ItemId}");
            if (shoppingCartItemDto.Quantity <= 0) throw new ArgumentException("Invalid shopping cart item quantity requested. Item quantity has to be higher than zero.");

            var shoppingCartItems = _dbContext.ShoppingCartItems.Where(cartItem => cartItem.ShoppingCartId == user.Cart.Id);

            if (shoppingCartItems.Any(cartItem => cartItem.ItemId == shoppingCartItemDto.ItemId))
            {
                var shoppingCartItem = shoppingCartItems.First(cartItem => cartItem.ItemId == shoppingCartItemDto.ItemId);
                shoppingCartItem.Quantity += shoppingCartItemDto.Quantity;
            }
            else
            {
                user.Cart.ShoppingCartItems.Add(_mapper.Map<ShoppingCartItem>(shoppingCartItemDto));
            }
            await _dbContext.SaveChangesAsync();
        }
        public async Task<ShoppingCartDto> GetAsync(string? currencyName)
        {
            int userId = _userContextService.GetUserId();
            User user = await _userServices.GetAsync(userId);
            
            decimal exchangeRate = currencyName == null? 1.00M : await _exchangeRatesServices.GetExchangeRateAsync(currencyName);
            if (currencyName == null) currencyName = "USD";

            var shoppingCartItems = await _dbContext.ShoppingCartItems.Where(cartItem => cartItem.ShoppingCartId == user.Cart.Id).ToListAsync();
            var shopItemDtos = new List<ShopItemDto>();
            foreach (var cartItem in shoppingCartItems)
            {
                var shopItem = await _dbContext.ShopItems.FindAsync(cartItem.ItemId);
                var shopItemDto = _mapper.Map<ShopItemDto>(shopItem);
                shopItemDto.Quantity = cartItem.Quantity;
                shopItemDto.Price = shopItemDto.Price* exchangeRate;
                shopItemDto.PriceCurrency = currencyName;

                shopItemDtos.Add(shopItemDto);
            }
            var shoppingCartDto = new ShoppingCartDto()
            {
                Id = user.Cart.Id,
                ShopItems = shopItemDtos,
            };
            return shoppingCartDto; 
        }

        public async Task RemoveItemAsync(ShoppingCartItemDto shoppingCartItemDto)
        {
            int userId = _userContextService.GetUserId();
            User user = await _userServices.GetAsync(userId);

            var shoppingCartItem = await _dbContext.ShoppingCartItems
                .FirstOrDefaultAsync(cartItem => cartItem.ShoppingCartId == user.Cart.Id && cartItem.ItemId == shoppingCartItemDto.ItemId);
            if (shoppingCartItem == null) throw new NotFoundException("Could not find shopping cart item.");

            var shoppingCart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(shoppingCart => shoppingCart.UserId == user.Id);
            if (shoppingCart == null) throw new NotFoundException("Could not find shopping cart");

            shoppingCart.ShoppingCartItems.Remove(shoppingCartItem);
            await _dbContext.SaveChangesAsync();
        }   

        public async Task UpdateItemAsync(ShoppingCartItemDto shoppingCartItemDto)
        {
            int userId = _userContextService.GetUserId();
            User user = await _userServices.GetAsync(userId);
            var updatedCartItem = user.Cart.ShoppingCartItems.Find(shoppingCartItem => shoppingCartItem.ItemId == shoppingCartItemDto.ItemId);
            if (updatedCartItem == null) throw new NotFoundException($"Can't find shop item no. {shoppingCartItemDto.ItemId}");
            if (shoppingCartItemDto.Quantity <= 0) throw new ArgumentException("Invalid shopping cart item quantity requested. Updated quantity has to be higher than zero.");
            updatedCartItem.Quantity = shoppingCartItemDto.Quantity;
            await _dbContext.SaveChangesAsync();
        }
   
    }
}
