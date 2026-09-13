using ToDoApp.Interfaces.DTOs;
using ToDoApp.Interfaces.Entities;
using ToDoApp.Interfaces.Repositories;
using ToDoApp.Interfaces.Services;

namespace ToDoApp.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync(int userId)
        {
            var categories = await _categoryRepository.FindAsync(c => c.UserId == userId);
            return categories.Select(c => new CategoryDto(c.Id, c.Name)).ToList();
        }

        public async Task<CategoryDto> CreateCategoryAsync(int userId, CreateCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                UserId = userId
            };

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();

            return new CategoryDto(category.Id, category.Name);
        }

        public async Task DeleteCategoryAsync(int userId, int categoryId)
        {
            var categories = await _categoryRepository.FindAsync(c => c.Id == categoryId && c.UserId == userId);
            var category = categories.FirstOrDefault() ?? throw new Exception("Категорію не знайдено");

            _categoryRepository.Delete(category); // Заміни на Remove, якщо в IRepository він називається так
            await _categoryRepository.SaveChangesAsync();
        }
    }
}