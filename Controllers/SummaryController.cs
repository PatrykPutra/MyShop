using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Models;
using MyShop.Services;

namespace MyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class SummaryController : ControllerBase
    {
        private readonly ISummaryServices _services;
        public SummaryController(ISummaryServices services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetSummary()
        {
            var result = await _services.GetAsync();
            return Ok(result);
        }
        
    }
}
