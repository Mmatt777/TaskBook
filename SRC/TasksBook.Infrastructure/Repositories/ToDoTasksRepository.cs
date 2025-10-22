using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using TasksBook.Application.ToDoTasks.DTOS;
using TasksBook.Domain.Entities;
using TasksBook.Domain.Repositories;
using TasksBook.Infrastructure.Persistens;

namespace TasksBook.Infrastructure.Repositories
{
    public class ToDoTasksRepository(TasksBookDbContext dbContext) : IToDoTasksRepository
    {
        public async Task<IEnumerable<ToDoTask>> GetAllAsync()
        {
            var tasks = await dbContext.Tasks.ToListAsync();
            return tasks;
        }

        public async Task<ToDoTask?> GetTaskByIdAsync(Guid Id)
        {
            var task = await dbContext.Tasks.FirstOrDefaultAsync(c => c.Id == Id);

            return task;
        }
        
        public async Task<IEnumerable<ToDoTask>> GetIncomingTasks(DateTime start, DateTime end)
        {
            var tasks = await dbContext.Tasks
                .Where(c => c.ExpiresAt >= start && c.ExpiresAt < end && c.IsDone == false)
                .OrderBy(c => c.ExpiresAt)
                .ToListAsync();

            return tasks;
        }

        public async Task<Guid> CreateTaskAsync(ToDoTask toDoTask)
        {
            dbContext.Tasks.Add(toDoTask);
            await dbContext.SaveChangesAsync();

            return toDoTask.Id;
        }

        public async Task DeleteTaskAsync(ToDoTask toDoTask)
        {
            dbContext.Remove(toDoTask);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateTaskAsync(ToDoTask toDoTask)
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
