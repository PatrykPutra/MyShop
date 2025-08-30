using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Models;
using MyShop.Services;
using System.Security.Claims;


namespace MyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ItemCategoryController : ControllerBase
    {
        private readonly IItemCategoryServices _services;
        public ItemCategoryController(IItemCategoryServices services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await _services.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
           
            var categoryDto = await _services.GetByIdAsync(id);
            return Ok(categoryDto);
           
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] CreateItemCategoryDto newItemCategoryDto)
        {
            
            var result = await _services.CreateAsync(newItemCategoryDto);
            return Created($"api/ItemCategory/{result}",null); // To jest bardzo ok ale szczerze powiedziawszy Created jest bardzo rzadko używane. Najczęsciej zwraca się puste Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateItemCategoryDto itemCategoryDto)
        {
            await _services.UpdateAsync(id, itemCategoryDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _services.DeleteAsync(id);
            return Ok();
          
        }
        
    }
}
