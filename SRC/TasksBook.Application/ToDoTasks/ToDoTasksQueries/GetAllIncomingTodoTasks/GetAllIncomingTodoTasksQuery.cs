using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksBook.Application.ToDoTasks.DTOS;
using TasksBook.Domain.Constants;

namespace TasksBook.Application.ToDoTasks.ToDoTasksQueries.GetAllIncomingTodoTasks
{
    public record class GetAllIncomingTodoTasksQuery(IncomingPeriod Period) : IRequest<IEnumerable<ToDoTaskDto>>
    {
    }
}
