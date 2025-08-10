using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop.Models;
using MyShop.Services;

namespace MyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderServices _services;
        public OrdersController(IOrderServices services)
        {
            _services=services;
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateOrderDto newOrderDto)
        {
            
            await _services.AddAsync(newOrderDto);
            return Ok();
           
        }
    }
}
