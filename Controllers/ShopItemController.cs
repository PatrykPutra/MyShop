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
            try
            {
                var result = await _services.CreateAsync(shopItemDto);
                return Created($"api/ShopItem/{result}",null);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemByIdAsync(int id,[FromQuery]string currencyName)
        {
            try
            {
                var result = await _services.GetByIdAsync(id, currencyName);
                return Ok(result);
            }
            catch(ArgumentOutOfRangeException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpGet("ByCategory")]
        public async Task<IActionResult> GetItemsByCategoryAsync(int categoryId, [FromQuery] string currencyName)
        {
            try
            {
                var result = await _services.GetByCategoryAsync(categoryId, currencyName);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("All")]
        public async Task<IActionResult> GetItemsByCategoryAsync([FromQuery] string currencyName)
        {
            try
            {
                var result = await _services.GetAllAsync(currencyName);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItemAsync(int id, [FromBody] CreateShopItemDto updatedShopItem)
        {
            try
            {
                await _services.UpdateAsync(id, updatedShopItem);
                return NoContent();
            }
            catch(ArgumentOutOfRangeException ex)
            {
                return NotFound(ex.Message);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemAsync(int id,[FromBody]string token)
        {
            try
            {
                await _services.DeleteAsync(id,token);
                return NoContent();
            }
            catch(ArgumentOutOfRangeException ex)
            {
                return NotFound(ex.Message);
            }
           
        }

    }
}
