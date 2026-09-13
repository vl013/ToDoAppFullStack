namespace ToDoApp.Interfaces.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        public User? User { get; set; }

        // Ця властивість необхідна для зв'язку WithMany(c => c.Tasks)
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}