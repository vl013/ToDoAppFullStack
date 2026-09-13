namespace ToDoApp.Interfaces.DTOs
{
    public record CategoryDto(int Id, string Name);
    public record CreateCategoryDto(string Name);
}