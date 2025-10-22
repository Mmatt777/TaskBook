using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TasksBook.Domain.Entities;
using TasksBook.Domain.Exceptions;
using TasksBook.Domain.Repositories;

namespace TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTaskMarkAsDone
{
    public class ToDoTaskMarkAsDoneCommandHandler(ILogger<ToDoTaskMarkAsDoneCommandHandler> logger,
        IToDoTasksRepository toDoTasksRepository,
        IMapper mapper) 
        : IRequestHandler<ToDoTaskMarkAsDoneCommand, Unit>
    {
        public async Task<Unit> Handle(ToDoTaskMarkAsDoneCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Marking task as done with id:{request.Id}");
            var task = await toDoTasksRepository.GetTaskByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(ToDoTask), request.Id.ToString());

            var markedTask = mapper.Map(request, task);

            markedTask.PercentComplete = 100;
            markedTask.CompletedAt = DateTime.UtcNow;
            markedTask.IsDone = true;

            await toDoTasksRepository.UpdateTaskAsync(markedTask);

            return Unit.Value;
        }
    }
}
