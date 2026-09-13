using ToDoApp.Interfaces.DTOs;
using ToDoApp.Interfaces.Entities;
using ToDoApp.Interfaces.Repositories;
using ToDoApp.Interfaces.Services;

namespace ToDoApp.Services
{
    public class TaskService : ITaskService
    {
        private readonly IRepository<TaskItem> _taskRepository; // Dependency Injection Repository

        public TaskService(IRepository<TaskItem> taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskItemDto>> GetTasksAsync(int userId, int page, int pageSize, int? categoryId)
        {
            // Фільтрація по користувачу та категорії (Feature 5)
            var tasks = await _taskRepository.FindAsync(t => 
                t.UserId == userId && 
                (!categoryId.HasValue || t.CategoryId == categoryId.Value));

            // Пагінація (Feature 4)
            return tasks
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TaskItemDto(t.Id, t.Title, t.Description, t.IsCompleted, t.CreatedAt, t.CategoryId)) // DTO
                .ToList();
        }

        public async Task<TaskItemDto> CreateTaskAsync(int userId, CreateTaskDto dto)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                UserId = userId,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            };

            await _taskRepository.AddAsync(task);
            await _taskRepository.SaveChangesAsync();

            return new TaskItemDto(task.Id, task.Title, task.Description, task.IsCompleted, task.CreatedAt, task.CategoryId);
        }

        public async Task UpdateTaskAsync(int userId, int taskId, UpdateTaskDto dto)
        {
            var tasks = await _taskRepository.FindAsync(t => t.Id == taskId && t.UserId == userId);
            var task = tasks.FirstOrDefault() ?? throw new Exception("Завдання не знайдено");

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.IsCompleted = dto.IsCompleted;
            task.CategoryId = dto.CategoryId;

            await _taskRepository.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(int userId, int taskId)
        {
            var tasks = await _taskRepository.FindAsync(t => t.Id == taskId && t.UserId == userId); // Пошук завдання
            var task = tasks.FirstOrDefault() ?? throw new Exception("Завдання не знайдено"); // ?? Перевіряє ліву частину

            _taskRepository.Delete(task);
            await _taskRepository.SaveChangesAsync(); // Збереження оновленних дій
        }
    }
}