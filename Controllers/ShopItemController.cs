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
        private readonly IShopItemServices _services;
        
        public ShopItemController(IShopItemServices services)
        {
            _services = services;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] CreateShopItemDto shopItemDto)
        {
          
            var result = await _services.CreateAsync(shopItemDto);
            return Created($"api/ShopItem/{result}",null);
           
            
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemByIdAsync(int id,[FromQuery]string currencyName)
        {
            
            var result = await _services.GetByIdAsync(id, currencyName);
            return Ok(result);
  
        }
        [HttpGet("ByCategory")]
        public async Task<IActionResult> GetItemsByCategoryAsync(int categoryId, [FromQuery] string currencyName)
        {
            
            var result = await _services.GetByCategoryAsync(categoryId, currencyName);
            return Ok(result);
           
        }
        [HttpGet("All")]
        public async Task<IActionResult> GetItemsByCategoryAsync([FromQuery] string currencyName)
        {
            
            var result = await _services.GetAllAsync(currencyName);
            return Ok(result);
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItemAsync(int id, [FromBody] CreateShopItemDto updatedShopItem)
        {
           
            await _services.UpdateAsync(id, updatedShopItem);
            return NoContent();
           
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemAsync(int id,[FromBody]string token)
        {
           
            await _services.DeleteAsync(id,token);
            return NoContent();
           
           
        }

    }
}
