using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop.Models;
using MyShop.Services;
using System.Security.Authentication;
using System.Security.Claims;
namespace MyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ShoppingCartController : ControllerBase
    {
        private readonly MyShopDbContext _dbContext;
        private readonly IShoppingCartServices _shoppingCartServices;

        public ShoppingCartController(MyShopDbContext dbContext, IShoppingCartServices shoppingCartServices)
        {
            _dbContext = dbContext;
            _shoppingCartServices = shoppingCartServices;
        }
        [HttpPost("AddItem")]
        public async Task<IActionResult> AddItemAsync([FromBody]ShoppingCartItemDto shoppingCartItemDto)
        {

            await _shoppingCartServices.AddItemAsync(shoppingCartItemDto);
            return Ok();
            
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery]string? currencyName)
        {
            var result = await _shoppingCartServices.GetAsync(currencyName!);
            return Ok(result);
        }
        [HttpPut("RemoveItem")]
        public async Task<IActionResult> RemoveItemAsync([FromBody] ShoppingCartItemDto shoppingCartItemDto)
        {

            await _shoppingCartServices.RemoveItemAsync(shoppingCartItemDto);
            return Ok();
            
        }
        [HttpPut("UpdateItem")]
        public async Task<IActionResult> UpdateItemAsync([FromBody] ShoppingCartItemDto shoppingCartItemDto)
        {
            await _shoppingCartServices.UpdateItemAsync(shoppingCartItemDto);
            return Ok();
        }

    }
}
