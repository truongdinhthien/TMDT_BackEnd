using Microsoft.AspNetCore.Mvc;
using BookApi.Core.Fillter;
using BookApi.Core;
using BookApi.Services;
using BookApi.Core.Entity;
using System.Threading.Tasks;

namespace BookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        public CategoryController (CategoryService categoryService)
        {
            _categoryService = categoryService;
        } 
        [HttpGet]
        public async Task<IActionResult> GetAll ( [FromQuery] FillterCategory fillterCategory)
        {
            var result = await _categoryService.GetCategoryAsync(fillterCategory);
            return result;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            return category;
        }
    }
}