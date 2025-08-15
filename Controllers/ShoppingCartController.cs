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
            int userId = int.Parse(User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
            await _shoppingCartServices.AddItemAsync(shoppingCartItemDto,userId);
            return Ok();
            
        }
        [HttpPut("RemoveItem")]
        public async Task<IActionResult> RemoveItemAsync([FromBody] ShoppingCartItemDto shoppingCartItemDto)
        {
            int userId = int.Parse(User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
            await _shoppingCartServices.RemoveItemAsync(shoppingCartItemDto, userId);
            return Ok();
            
        }

    }
}
