using ToDoApp.Interfaces.DTOs;

namespace ToDoApp.Interfaces.Services

{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItemDto>> GetTasksAsync(int userId, int page, int pageSize, int? categoryId);
        Task<TaskItemDto> CreateTaskAsync(int userId, CreateTaskDto dto);
        Task UpdateTaskAsync(int userId, int taskId, UpdateTaskDto dto);
        Task DeleteTaskAsync(int userId, int taskId);
    }
}