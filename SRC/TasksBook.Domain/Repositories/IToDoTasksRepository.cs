using TasksBook.Domain.Entities;

namespace TasksBook.Domain.Repositories;

public interface IToDoTasksRepository
{
    Task<IEnumerable<ToDoTask>> GetAllAsync();
    Task<ToDoTask?> GetTaskByIdAsync(Guid Id);
    Task<IEnumerable<ToDoTask>> GetIncomingTasks(DateTime start, DateTime end);
    Task<Guid> CreateTaskAsync(ToDoTask toDoTask);

    Task DeleteTaskAsync(ToDoTask toDoTask);
    Task UpdateTaskAsync(ToDoTask toDoTask);
}
