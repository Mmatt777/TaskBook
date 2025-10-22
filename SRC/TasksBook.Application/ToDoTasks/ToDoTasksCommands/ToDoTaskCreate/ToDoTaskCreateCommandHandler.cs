using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TasksBook.Domain.Entities;
using TasksBook.Domain.Repositories;

namespace TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTasksCreate
{
    public class ToDoTaskCreateCommandHandler(ILogger<ToDoTaskCreateCommandHandler> logger,
        IToDoTasksRepository toDoTasksRepository,
        IMapper mapper) 
        : IRequestHandler<ToDoTaskCreateCommand, Guid>
    {
        public async Task<Guid> Handle(ToDoTaskCreateCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Cerating new task.");

            var newTask = mapper.Map<ToDoTask>(request);

            newTask.IsDone = false;
            newTask.CreatedAt = DateTime.UtcNow;

            var id = await toDoTasksRepository.CreateTaskAsync(newTask);

            return id;
        }
    }
}
