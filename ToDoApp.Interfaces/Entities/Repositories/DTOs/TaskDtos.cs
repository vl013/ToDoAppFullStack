namespace ToDoApp.Interfaces.DTOs
{
    public record TaskItemDto(int Id, string Title, string? Description, bool IsCompleted, DateTime CreatedAt, int? CategoryId);
    public record CreateTaskDto(string Title, string? Description, int? CategoryId);
    public record UpdateTaskDto(string Title, string? Description, bool IsCompleted, int? CategoryId);
}