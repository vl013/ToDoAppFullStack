using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoApp.Interfaces.DTOs;
using ToDoApp.Interfaces.Services;

namespace ToDoApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController (ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        [HttpGet]
        public async Task<IActionResult> GetCategories() => Ok(await _categoryService.GetCategoriesAsync(GetUserId()));

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto) => Ok(await _categoryService.CreateCategoryAsync(GetUserId(), dto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(GetUserId(), id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}