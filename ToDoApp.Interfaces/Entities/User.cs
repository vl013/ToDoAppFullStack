using System.Collections.Generic;

namespace ToDoApp.Interfaces.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}