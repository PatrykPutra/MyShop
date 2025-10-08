using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Client;
using MyShop.Data;
using MyShop.Models;
using MyShop.Services;
using System.Security.Claims;


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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct([FromBody] CreateShopItemDto shopItemDto)
        {
            var result = await _services.CreateAsync(shopItemDto);
            return Ok();
   
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemByIdAsync(int id,[FromQuery]string currencyName)
        {
            
            var result = await _services.GetByIdAsync(id, currencyName);
            return Ok(result);
  
        }
      
        [HttpGet("All")]
        public async Task<IActionResult> GetItemsByCategoryAsync([FromQuery] ShopItemQuery query)
        {
            
            var result = await _services.GetAllAsync(query.CategoryId,query.CurrencyName);
            return Ok(result);
            
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateItemAsync(int id, [FromBody] CreateShopItemDto updatedShopItem)
        {
            await _services.UpdateAsync(id, updatedShopItem);
            return NoContent();  
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteItemAsync(int id)
        {
            await _services.DeleteAsync(id);
            return NoContent();
   
        }

    }
}
