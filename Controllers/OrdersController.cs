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
            try
            {
                await _services.AddAsync(newOrderDto);
                return Ok();
            }
            catch(ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
