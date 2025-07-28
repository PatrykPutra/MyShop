using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Client;
using MyShop.Data;
using MyShop.Models;
using MyShop.Services;


namespace MyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShopItemController : ControllerBase
    {
        private readonly MyShopDbContext _dbContext;
        private readonly ExchangeRatesServices _exchangeRatesServices;
        
        public ShopItemController(MyShopDbContext dbContext)
        {
            _dbContext = dbContext;
            _exchangeRatesServices = new(dbContext);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ShopItemPostDto shopItemDto)
        {
            if (shopItemDto == null)  return BadRequest("Invalid product data"); 

            ItemCategory? category = await _dbContext.ItemCategories.FindAsync(shopItemDto.CategoryId);
            if (category == null)  return BadRequest("Invalid product category");

            ShopItem shopItem = new ShopItem
            {
                Name = shopItemDto.Name,
                Text = shopItemDto.Text,
                PriceUSD = shopItemDto.Price,
                Quantity = shopItemDto.Quantity,
                Category = category,
                CategoryId = category.Id
            };
            
            _dbContext.ShopItems.Add(shopItem);
            await _dbContext.SaveChangesAsync();
            
            return CreatedAtAction("AddProduct",new {id = shopItem.Id},shopItem);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemByIdAsync(int id,[FromQuery]string currencyName)
        {
            ShopItem? shopItem = await _dbContext.ShopItems.Include(shopItem=>shopItem.Category).FirstOrDefaultAsync(shopItem=>shopItem.Id==id);
            decimal exchangeRate;
            if (shopItem == null) return NotFound();
            try
            {
                exchangeRate = await _exchangeRatesServices.GetExchangeRateAsync(currencyName);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            ShopItemGetDto shopItemDto = new()
            {
                Name = shopItem.Name,
                Text = shopItem.Text,
                Price = shopItem.PriceUSD * exchangeRate,
                PriceCurrency = currencyName,
                CategoryId = shopItem.CategoryId,
                Quantity = shopItem.Quantity,
            };
            

            return Ok(shopItemDto);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItemAsync(int id, [FromBody] ShopItemPostDto updatedShopItem)
        {
            
            ShopItem? existingShopItem = await _dbContext.ShopItems.FindAsync(id);
            if(existingShopItem == null) return NotFound();
            ItemCategory? category = await _dbContext.ItemCategories.FindAsync(updatedShopItem.CategoryId);
            if (category == null) return BadRequest("Invalid product category");
            
            existingShopItem.Name = updatedShopItem.Name;
            existingShopItem.Text = updatedShopItem.Text;
            existingShopItem.PriceUSD = updatedShopItem.Price;
            existingShopItem.CategoryId = updatedShopItem.CategoryId;
            existingShopItem.Quantity = updatedShopItem.Quantity;

            _dbContext.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemAsync(int id)
        {
            ShopItem? shopItem = await _dbContext.ShopItems.FindAsync(id);
            if (shopItem == null) return NotFound();
            _dbContext.ShopItems.Remove(shopItem);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

    }
}
