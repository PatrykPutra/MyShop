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
            await _services.AddAsync();
            return Ok(); 
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string currencyName)
        {
            var result = await _services.GetAsync(currencyName);
            return Ok(new { Orders = result });
        }
    }
}
