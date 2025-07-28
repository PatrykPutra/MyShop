using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Models;

namespace MyShop.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly MyShopDbContext _dbContext;
        public OrderServices(MyShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        ///<exception cref="ArgumentException">
        ///Thrown if newOrderDto is null or Items property of newOrderDto is empty List.
        ///</exception>
        ///<exception cref="ArgumentOutOfRangeException">
        ///Thrown if can't find ShopItem in database or ShopItem quantity is below or equal zero.
        ///</exception>
        public async Task AddAsync(CreateOrderDto newOrderDto)
        
        {
            if (newOrderDto == null || newOrderDto.Items.Count == 0) throw new ArgumentException("Item list is empty");

            List<OrderItem> orderItems = new List<OrderItem>();
            decimal totalPriceUSD = 0;

            foreach (var item in newOrderDto.Items)
            {
                ShopItem? shopItem = await _dbContext.ShopItems.FindAsync(item.ShopItemId);
                if (shopItem == null) throw new ArgumentOutOfRangeException($"Shop item no. {item.ShopItemId} does not exist");
                OrderItem orderItem = new OrderItem
                {
                    Name = shopItem.Name,
                    ShopItemId = shopItem.Id,
                    PriceUSD = shopItem.PriceUSD,
                };
                orderItems.Add(orderItem);
                totalPriceUSD += shopItem.PriceUSD;
                shopItem.Quantity--;
                if (shopItem.Quantity <= 0) throw new ArgumentOutOfRangeException($"Not enough {shopItem.Name} in shop storage.");
            }
            Order newOrder = new Order
            {
                CreationDate = DateTime.Now,
                Items = orderItems,
                TotalPriceUSD = totalPriceUSD
            };
            _dbContext.Orders.Add(newOrder);
            await _dbContext.SaveChangesAsync();
            
        }
    }
}
