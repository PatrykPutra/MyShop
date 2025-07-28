using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop.Models;

namespace MyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly MyShopDbContext _dbContext;
        public OrdersController(MyShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] OrderDto newOrderDto)
        {
            if (newOrderDto == null || newOrderDto.Items.Count == 0) return BadRequest("Item list is empty");

            List<OrderItem> orderItems = new List<OrderItem>();
            decimal totalPriceUSD = 0;

            foreach (var item in newOrderDto.Items)
            {
                ShopItem? shopItem = await _dbContext.ShopItems.FindAsync(item.ShopItemId);
                if (shopItem == null) return BadRequest($"Shop item no. {item.ShopItemId} does not exist");
                OrderItem orderItem = new OrderItem
                {
                    Name = shopItem.Name,
                    ShopItemId = shopItem.Id,
                    PriceUSD = shopItem.PriceUSD,
                };
                orderItems.Add(orderItem);
                totalPriceUSD += shopItem.PriceUSD;
                shopItem.Quantity--;
                if (shopItem.Quantity <= 0) return BadRequest($"Not enough {shopItem.Name} in shop storage.");
            }

            Order newOrder = new Order
            {
                CreationDate = DateTime.Now,
                Items = orderItems,
                TotalPriceUSD = totalPriceUSD
            };
            _dbContext.Orders.Add(newOrder);
            await _dbContext.SaveChangesAsync();
            return Ok(newOrder.Id);
        }
    }
}
