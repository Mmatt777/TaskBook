using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTaskDelete
{
    public record class ToDoTaskDeleteCommand(Guid Id) : IRequest<bool>
    {
    }
}
