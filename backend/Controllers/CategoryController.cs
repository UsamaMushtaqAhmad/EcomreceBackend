using backend.DTOs;
using backend.Services.NewFolder;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // ✅ Get all categories
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetAllCategories();
            return Ok(categories);
        }

        // ✅ Get category by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        // ✅ Add new category
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null || string.IsNullOrEmpty(categoryDto.CategoryName))
                return BadRequest("Invalid data.");

            await _categoryService.AddCategory(categoryDto);
            return Ok(new { Message = "Category added successfully" });
        }

        // ✅ Update category
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null || id != categoryDto.CategoryId)
                return BadRequest("Invalid data.");

            await _categoryService.UpdateCategory(categoryDto);
            return Ok(new { Message = "Category updated successfully" });
        }

        // ✅ Delete category
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategory(id);
            return Ok(new { Message = "Category deleted successfully" });
        }
    }
}