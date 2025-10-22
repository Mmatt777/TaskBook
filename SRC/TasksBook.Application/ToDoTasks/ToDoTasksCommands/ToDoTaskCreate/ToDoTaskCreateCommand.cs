using MediatR;

namespace TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTasksCreate;

public class ToDoTaskCreateCommand() : IRequest<Guid>
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
}
   

