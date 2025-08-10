using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Models;
using MyShop.Services;


namespace MyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            try
            {
                var categoryDto = await _services.GetByIdAsync(id);
                return Ok(categoryDto);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateItemCategoryDto newItemCategoryDto)
        {
            var result = await _services.CreateAsync(newItemCategoryDto);
            return Created($"api/ItemCategory/{result}",null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateItemCategoryDto itemCategoryDto)
        {
            try
            {
                await _services.UpdateAsync(id, itemCategoryDto);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromBody] string token)
        {
            try
            {
                await _services.DeleteAsync(id,token);
                return Ok();
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
