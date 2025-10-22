using Microsoft.EntityFrameworkCore;
using TasksBook.Domain.Entities;


namespace TasksBook.Infrastructure.Persistens;

public class TasksBookDbContext(DbContextOptions<TasksBookDbContext> options) : DbContext(options)
{
    public DbSet<ToDoTask> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ToDoTask>(a =>
        {
            a.HasKey(c => c.Id);
            a.Property(c => c.Name).IsRequired().HasMaxLength(100);
            a.Property(c => c.Description).HasMaxLength(1000);
        });
           
        
    }
}
