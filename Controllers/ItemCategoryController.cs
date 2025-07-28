using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Models;


namespace MyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemCategoryController : ControllerBase
    {
        private readonly MyShopDbContext _dbContext;
        private readonly IMapper _mapper;
        public ItemCategoryController(IMapper mapper,MyShopDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<ItemCategory> categories = await _dbContext.ItemCategories.ToListAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            ItemCategory? category = await _dbContext.ItemCategories.Include(category => category.ShopItems).FirstOrDefaultAsync(category=> category.Id == id);
            if (category == null) return NotFound();

            return Ok(_mapper.Map<ItemCategoryDto>(category));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ItemCategoryDto newItemCategoryDto)
        {
            ItemCategory newItemCategory = _mapper.Map<ItemCategory>(newItemCategoryDto);
            _dbContext.ItemCategories.Add(newItemCategory);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(Add), newItemCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ItemCategoryDto itemCategoryDto)
        {
            ItemCategory? itemCategory = await _dbContext.ItemCategories.FindAsync(id);
            if (itemCategory == null) return NotFound();
            if (itemCategory.Id == 1) return BadRequest("Category Other cannot be removed");
            itemCategory.Name = itemCategoryDto.Name;
            await _dbContext.SaveChangesAsync();
            return Ok(itemCategory);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            ItemCategory? category = await _dbContext.ItemCategories
                .Include(category=>category.ShopItems)
                .FirstOrDefaultAsync(category=>category.Id==id);
            if (category == null) return NotFound();
            if (category.Id == 1) return BadRequest("Category Other cannot be removed");
            foreach (var item in category.ShopItems)
            {
                item.CategoryId = 1;
            }
            
            _dbContext.ItemCategories.Remove(category);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        
    }
}
