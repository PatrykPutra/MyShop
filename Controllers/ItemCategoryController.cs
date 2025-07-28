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
        public IActionResult Get()
        {
            var categories = _services.GetAll();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var categoryDto = _services.GetById(id);
                return Ok(categoryDto);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Add([FromBody] CreateItemCategoryDto newItemCategoryDto)
        {
            var result = _services.Create(newItemCategoryDto);
            return Created($"api/ItemCategory/{result}",null);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CreateItemCategoryDto itemCategoryDto)
        {
            try
            {
                _services.Update(id, itemCategoryDto);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _services.Delete(id);
                return Ok();
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
