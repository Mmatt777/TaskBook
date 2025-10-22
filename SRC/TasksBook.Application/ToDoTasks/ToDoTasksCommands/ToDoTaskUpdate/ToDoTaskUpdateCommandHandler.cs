using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksBook.Domain.Entities;
using TasksBook.Domain.Exceptions;
using TasksBook.Domain.Repositories;

namespace TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTaskUpdate
{
    public class ToDoTaskUpdateCommandHandler(ILogger<ToDoTaskUpdateCommandHandler> logger,
        IToDoTasksRepository toDoTasksRepository,
        IMapper mapper) 
        : IRequestHandler<ToDoTaskUpdateCommand, Unit>
    {
        public async Task<Unit> Handle(ToDoTaskUpdateCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Updating task with id:{request.Id}");
            var task = await toDoTasksRepository.GetTaskByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(ToDoTask), request.Id.ToString());

            var updatedTask = mapper.Map(request, task);

            updatedTask.IsDone = false;      
            updatedTask.UpdatedAt = DateTime.UtcNow;

            await toDoTasksRepository.UpdateTaskAsync(updatedTask);
            return Unit.Value;
        }
    }
}
