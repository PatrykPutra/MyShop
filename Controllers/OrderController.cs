using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop.Models;
using MyShop.Services;
using System.Security.Claims;

namespace MyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _services;
        public OrderController(IOrderServices services)
        {
            _services=services;
        }
        [HttpPost]
        public async Task<IActionResult> Add()
        {
            int userId = int.Parse(User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
            await _services.AddAsync(userId);
            return Ok();
           
        }
    }
}
