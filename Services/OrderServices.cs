using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Entities;
using MyShop.Models;
using System.Security.Authentication;

namespace MyShop.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly MyShopDbContext _dbContext;
        private readonly IUserServices _userServices;
        private readonly IUserContextService _userContextService;
        private readonly IExchangeRatesServices _exchangeRatesServices;
        private readonly IMapper _mapper;
        public OrderServices(MyShopDbContext dbContext, IUserServices userServices,IUserContextService userContextService, IExchangeRatesServices exchangeRatesServices, IMapper mapper)
        {
            _dbContext = dbContext;
            _userServices = userServices;
            _userContextService = userContextService;
            _exchangeRatesServices = exchangeRatesServices;
            _mapper = mapper;
        }
        
        public async Task AddAsync()
        {
            int userId = _userContextService.GetUserId();
            User user = await _userServices.GetAsync(userId);

            var shoppingCartItems = new List<ShoppingCartItem>(user.Cart.ShoppingCartItems);
            if (shoppingCartItems.Count == 0) throw new ArgumentException("Cart is empty");

            List<OrderItem> orderItems = new List<OrderItem>();
            decimal totalPriceUSD = 0;

            foreach (var cartItem in shoppingCartItems)
            {
                ShopItem? shopItem = await _dbContext.ShopItems.FindAsync(cartItem.ItemId);
                if (shopItem == null) throw new ArgumentException($"Shop item no. {cartItem.ItemId} does not exist");
                if (shopItem.Quantity < cartItem.Quantity) throw new ArgumentException($"Not enough {shopItem.Name} in shop storage.");
                OrderItem orderItem = new OrderItem
                {
                    Name = shopItem.Name,
                    ShopItemId = shopItem.Id,
                    PriceUSD = shopItem.PriceUSD,
                    Quantity = cartItem.Quantity,
                };
                orderItems.Add(orderItem);
                
                totalPriceUSD += shopItem.PriceUSD* cartItem.Quantity;
                shopItem.Quantity = shopItem.Quantity - cartItem.Quantity;
                
            }
            
            Order newOrder = new Order
            {
                CreationDate = DateTime.Now,
                Items = orderItems,
                TotalPriceUSD = totalPriceUSD,
                User = user,
                UserId= user.Id,
                StatusId=1,
            };

            _dbContext.Orders.Add(newOrder);
            user.Cart.ShoppingCartItems.Clear();
            await _dbContext.SaveChangesAsync();
            
        }
        public async Task<List<OrderDto>> GetAsync(string currencyName)
        {
            int userId = _userContextService.GetUserId();
            var orders = await _dbContext.Orders.Include(order=>order.Items).Where(order=>order.UserId == userId).ToListAsync();
            var exchangeRate = await _exchangeRatesServices.GetExchangeRateAsync(currencyName);
            var ordersDto = _mapper.Map<List<OrderDto>>(orders);
            foreach (var order in ordersDto) 
            {
                order.TotalPrice *= exchangeRate;
                order.CurrencyName = currencyName;
                foreach (var item in order.Items)
                {
                    item.Price *= exchangeRate;
                    item.CurrencyName = currencyName;
                }
            };

            return ordersDto;
        }
        // getAll
       
    }
}
