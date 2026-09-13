using Microsoft.EntityFrameworkCore;
using ToDoApp.Interfaces.Entities;

namespace ToDoApp.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Tables DbSet
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API
            modelBuilder.Entity<Category>()
            .HasOne(c => c.User) // one user
            .WithMany(u => u.Categories) // one user can have many categories
            .HasForeignKey(c => c.UserId) // hold hands across UserId
            .OnDelete(DeleteBehavior.Restrict); // Delete: Forbidden

            modelBuilder.Entity<TaskItem>() // task is ONE category
            .HasOne(t => t.Category) // in one category can be MANY tasks
            .WithMany(c => c.Tasks)
            .HasForeignKey(t => t.CategoryId) // hold hands across CategoryID
            .OnDelete(DeleteBehavior.SetNull); // null
        }
    }
}