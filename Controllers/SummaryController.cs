using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Models;

namespace MyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SummaryController : ControllerBase
    {
        private readonly MyShopDbContext _dbContext;
        public SummaryController(MyShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetSummary()
        {
            int numberOfProducts = await _dbContext.ShopItems.CountAsync();
            int numberOfOrders = await _dbContext.Orders.CountAsync();
            int allProductsQuantity = await _dbContext.ShopItems.Select(shopItem => shopItem.Quantity).SumAsync();
            decimal allOrdersTotalPriceUSD = await _dbContext.Orders.Select(order=>order.TotalPriceUSD).SumAsync();
            Summary summary = new()
            {
                NumberOfProducts = numberOfProducts,
                NumberOfOrders = numberOfOrders,
                AllProductsQuantity = allProductsQuantity,
                AllOrdersTotalPriceUSD = allOrdersTotalPriceUSD
            };
            return Ok(summary);
        }
        
    }
}
