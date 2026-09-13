using ToDoApp.Interfaces.DTOs; 

namespace ToDoApp.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync(int userId);
        Task<CategoryDto> CreateCategoryAsync(int userId, CreateCategoryDto dto);
        Task DeleteCategoryAsync(int userId, int categoryId);
    }
}