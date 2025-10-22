using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using TasksBook.Domain.Entities;
using TasksBook.Domain.Exceptions;
using TasksBook.Domain.Repositories;

namespace TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTaskDelete
{
    public class ToDoTaskDeleteCommandHandler(ILogger<ToDoTaskDeleteCommandHandler> logger,
        IToDoTasksRepository toDoTasksRepository) 
        : IRequestHandler<ToDoTaskDeleteCommand, bool>
    {
        public async Task<bool> Handle(ToDoTaskDeleteCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Deleting task with id: {request.Id}");
            var taskById = await toDoTasksRepository.GetTaskByIdAsync(request.Id);

                if (taskById == null)
                return false;

            await toDoTasksRepository.DeleteTaskAsync(taskById);

            return true;
        }
    }
}
