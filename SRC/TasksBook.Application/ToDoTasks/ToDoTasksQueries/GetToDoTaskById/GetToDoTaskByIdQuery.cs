using MediatR;
using TasksBook.Application.ToDoTasks.DTOS;

namespace TasksBook.Application.ToDoTasks.ToDoTasksQueries.GetToDoTaskById;

public record class GetToDoTaskByIdQuery(Guid Id) : IRequest<ToDoTaskDto>
{
}
