using MediatR;
using TasksBook.Application.ToDoTasks.DTOS;

namespace TasksBook.Application.ToDoTasks.ToDoTasksQueries.GetAllToDoTasks;

public class GetAllToDoTaskQuery : IRequest<IEnumerable<ToDoTaskDto>>
{

}
