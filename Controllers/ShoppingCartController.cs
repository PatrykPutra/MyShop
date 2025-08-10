using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop.Models;
using MyShop.Services;
using System.Security.Authentication;
namespace MyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<IActionResult> AddItemAsync(ShoppingCartItemDto shoppingCartItemDto)
        {
            
            await _shoppingCartServices.AddItemAsync(shoppingCartItemDto);
            return Ok();
            
        }
        [HttpPut("RemoveItem")]
        public async Task<IActionResult> RemoveItemAsync(ShoppingCartItemDto shoppingCartItemDto)
        {
            
            await _shoppingCartServices.RemoveItemAsync(shoppingCartItemDto);
            return Ok();
            
        }

    }
}
