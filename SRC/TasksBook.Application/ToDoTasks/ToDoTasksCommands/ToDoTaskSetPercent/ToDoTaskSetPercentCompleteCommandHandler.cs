using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TasksBook.Domain.Entities;
using TasksBook.Domain.Exceptions;
using TasksBook.Domain.Repositories;

namespace TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTaskSetPercent
{
    public class ToDoTaskSetPercentCompleteCommandHandler(ILogger<ToDoTaskSetPercentCompleteCommandHandler> logger,
        IToDoTasksRepository toDoTasksRepository,
        IMapper mapper)
        : IRequestHandler<ToDoTaskSetPercentCompleteCommand, Unit>
    {
        public async Task<Unit> Handle(ToDoTaskSetPercentCompleteCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Setting percent complete of task with id{request.Id}");
            var task = await toDoTasksRepository.GetTaskByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(ToDoTask), request.Id.ToString());

            mapper.Map(request, task);

            var percentValue = task.PercentComplete = request.PercentComplete;
            if (percentValue == 100)
            {
                task.IsDone = true;
                task.CompletedAt = DateTime.UtcNow;
            }

            await toDoTasksRepository.UpdateTaskAsync(task);

            return Unit.Value;
        }
    }
}
