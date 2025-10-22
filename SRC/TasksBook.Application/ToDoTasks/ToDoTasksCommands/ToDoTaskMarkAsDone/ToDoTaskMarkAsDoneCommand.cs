using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTaskMarkAsDone
{
    public class ToDoTaskMarkAsDoneCommand : IRequest<Unit>
    {
        public Guid Id { get; set;}
    }
}
