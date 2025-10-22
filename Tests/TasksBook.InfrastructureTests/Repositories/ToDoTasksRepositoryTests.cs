using Microsoft.EntityFrameworkCore;
using TasksBook.Domain.Entities;
using TasksBook.Infrastructure.Persistens;
using Xunit;

namespace TasksBook.Infrastructure.Repositories.Tests
{
    public class ToDoTasksRepositoryTests
    {
        private TasksBookDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<TasksBookDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            return new TasksBookDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllTasks()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();

            var tasks = new List<ToDoTask>()
            {
                new() { Id = Guid.NewGuid(), Name = "Task 1", Description = "Desc 1" },

                new() { Id = Guid.NewGuid(), Name = "Task 2", Description = "Desc 2" }
            };

            await dbContext.Tasks.AddRangeAsync(tasks);
            await dbContext.SaveChangesAsync();

            var repository = new ToDoTasksRepository(dbContext);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, t => t.Name == "Task 1");
            Assert.Contains(result, t => t.Name == "Task 2");
            Assert.Contains(result, t => t.Description == "Desc 1");
            Assert.Contains(result, t => t.Description == "Desc 2");

        }

        [Fact]
        public async Task GetTaskByIdAsync_ShouldReturnCorrectTask()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();

            var task = new ToDoTask() 
            { 
                Id = Guid.NewGuid(), 
                Name = "Find me", 
                Description = "Find desc" 
            };

            dbContext.Tasks.Add(task);
            await dbContext.SaveChangesAsync();

            var repository = new ToDoTasksRepository(dbContext);

            // Act
            var result = await repository.GetTaskByIdAsync(task.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Find me", result.Name);
            Assert.Equal("Find desc", result.Description);
        }

        [Fact]
        public async Task GetIncomingTasks_ShouldReturnOnlyUndoneTasksWithinRange()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var now = DateTime.UtcNow;

            var tasks = new List<ToDoTask>()
            {
                new() { Id = Guid.NewGuid(), Name = "Today", Description = "Today desc", ExpiresAt = now.AddHours(2), IsDone = false },

                new() { Id = Guid.NewGuid(), Name = "Tomorrow", Description = "Tomorrow desc", ExpiresAt = now.AddDays(1), IsDone = false },

                new() { Id = Guid.NewGuid(), Name = "DoneTask", Description = "Done task desc", ExpiresAt = now.AddHours(3), IsDone = true },

                new() { Id = Guid.NewGuid(), Name = "TooLate", Description = "Too late desc", ExpiresAt = now.AddDays(5), IsDone = false }
            };

            await dbContext.Tasks.AddRangeAsync(tasks);
            await dbContext.SaveChangesAsync();

            var repository = new ToDoTasksRepository(dbContext);
            var start = now;
            var end = now.AddDays(2);

            // Act
            var result = await repository.GetIncomingTasks(start, end);

            // Assert
            var resultList = result.ToList();
            Assert.Equal(2, resultList.Count);
            Assert.DoesNotContain(resultList, t => t.IsDone);
            Assert.All(resultList, t => Assert.True(t.ExpiresAt >= start && t.ExpiresAt < end));
            Assert.True(resultList.SequenceEqual(resultList.OrderBy(t => t.ExpiresAt)));
        }

        [Fact]
        public async Task CreateTaskAsync_ShouldAddTaskToDatabase()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var repository = new ToDoTasksRepository(dbContext);

            var newTask = new ToDoTask()
            {
                Id = Guid.NewGuid(),
                Name = "New task",
                Description = "New desc"            
            };

            // Act
            var id = await repository.CreateTaskAsync(newTask);

            // Assert
            var created = await dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            Assert.NotNull(created);
            Assert.Equal("New task", created.Name);
        }

        [Fact]
        public async Task DeleteTaskAsync_ShouldRemoveTaskFromDatabase()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();

            var task = new ToDoTask() 
            { 
                Id = Guid.NewGuid(), 
                Name = "To delete",
                Description = "Deleted desc"
            };
            dbContext.Tasks.Add(task);
            await dbContext.SaveChangesAsync();

            var repository = new ToDoTasksRepository(dbContext);

            // Act
            await repository.DeleteTaskAsync(task);

            // Assert
            var exists = await dbContext.Tasks.AnyAsync(t => t.Id == task.Id);
            Assert.False(exists);
        }

        [Fact]
        public async Task UpdateTaskAsync_ShouldSaveChangesToDatabase()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var task = new ToDoTask()
            { 
                Id = Guid.NewGuid(), 
                Name = "Old title",
                Description = "Older desc"
            };

            dbContext.Tasks.Add(task);
            await dbContext.SaveChangesAsync();

            task.Name = "Updated title";
            var repository = new ToDoTasksRepository(dbContext);

            // Act
            await repository.UpdateTaskAsync(task);

            // Assert
            var updated = await dbContext.Tasks.FirstAsync(t => t.Id == task.Id);
            Assert.Equal("Updated title", updated.Name);
        }
    }
}