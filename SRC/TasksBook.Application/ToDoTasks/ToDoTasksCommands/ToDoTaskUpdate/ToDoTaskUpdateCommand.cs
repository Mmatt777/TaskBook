using MediatR;

namespace TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTaskUpdate
{
    public class ToDoTaskUpdateCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
    }
}
