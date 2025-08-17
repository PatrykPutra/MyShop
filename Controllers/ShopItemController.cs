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
    [Authorize]
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
            int userId = int.Parse(User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _services.CreateAsync(shopItemDto,userId);
            return Created($"api/ShopItem/{result}",null);
           
            
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemByIdAsync(int id,[FromQuery]string currencyName)
        {
            
            var result = await _services.GetByIdAsync(id, currencyName);
            return Ok(result);
  
        }
        [HttpGet("ByCategory")]
        public async Task<IActionResult> GetItemsByCategoryAsync([FromQuery] int categoryId, [FromQuery] string currencyName)
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateItemAsync(int id, [FromBody] CreateShopItemDto updatedShopItem)
        {
            int userId = int.Parse(User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
            await _services.UpdateAsync(id, updatedShopItem,userId);
            return NoContent();
           
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteItemAsync(int id)
        {
            int userId = int.Parse(User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
            await _services.DeleteAsync(id, userId);
            return NoContent();
           
           
        }

    }
}
