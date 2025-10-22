
using TasksBook.Domain.Entities;

namespace TasksBook.Application.ToDoTasks.DTOS;

public class ToDoTaskDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
    public int PercentComplete { get; set; }
    public bool IsDone { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    public static ToDoTaskDto FromEntity(ToDoTask todoTask)
    {
        return new ToDoTaskDto()
        {
            Id = todoTask.Id,
            Name = todoTask.Name,
            Description = todoTask.Description,
            ExpiresAt = todoTask.ExpiresAt,
            PercentComplete = todoTask.PercentComplete,
            CreatedAt = todoTask.CreatedAt,
            UpdatedAt = todoTask.UpdatedAt,
            CompletedAt = todoTask.CompletedAt
        };
    }


}
