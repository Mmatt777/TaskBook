
using MediatR;

namespace TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTaskSetPercent
{
    public class ToDoTaskSetPercentCompleteCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public bool IsDone { get; set; }
        public int PercentComplete { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
