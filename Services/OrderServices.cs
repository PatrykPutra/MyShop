using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Models;
using System.Security.Authentication;

namespace MyShop.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly MyShopDbContext _dbContext;
        private readonly IUserServices _userServices;
        private readonly IUserAuthorizationServices _userAuthorizationServices;
        public OrderServices(MyShopDbContext dbContext, IUserServices userServices,IUserAuthorizationServices userAuthorizationServices)
        {
            _dbContext = dbContext;
            _userAuthorizationServices = userAuthorizationServices;
            _userServices = userServices;
        }
        
        public async Task AddAsync(CreateOrderDto newOrderDto)
        {
            if (newOrderDto == null) throw new ArgumentException("Invalid request parameter.");
            User user = await _userServices.GetAsync(newOrderDto.Token);

            List<int> orderItemsIds = new List<int>(user.Cart.ShopItemsIds);
            if (orderItemsIds.Count == 0) throw new ArgumentException("Cart is empty");

            List<OrderItem> orderItems = new List<OrderItem>();
            decimal totalPriceUSD = 0;

            foreach (var itemId in orderItemsIds)
            {
                ShopItem? shopItem = await _dbContext.ShopItems.FindAsync(itemId);
                if (shopItem == null) throw new ArgumentException($"Shop item no. {itemId} does not exist");

                OrderItem orderItem = new OrderItem
                {
                    Name = shopItem.Name,
                    ShopItemId = shopItem.Id,
                    PriceUSD = shopItem.PriceUSD,
                };

                orderItems.Add(orderItem);
                totalPriceUSD += shopItem.PriceUSD;
                shopItem.Quantity--; // move to ShopItemServices
                if (shopItem.Quantity <= 0) throw new ArgumentException($"Not enough {shopItem.Name} in shop storage.");
            }
            
            Order newOrder = new Order
            {
                CreationDate = DateTime.Now,
                Items = orderItems,
                TotalPriceUSD = totalPriceUSD,
                User = user,
                UserId= user.Id,
            };

            _dbContext.Orders.Add(newOrder);
            user.Cart.ShopItemsIds.Clear();
            await _dbContext.SaveChangesAsync();
            
        }
      
    }
}
