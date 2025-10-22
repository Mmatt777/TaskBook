using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TasksBook.Application.ToDoTasks.DTOS;
using TasksBook.Domain.Exceptions;
using TasksBook.Domain.Repositories;

namespace TasksBook.Application.ToDoTasks.ToDoTasksQueries.GetToDoTaskById;

public class GetToDoTaskByIdQueryHandler(ILogger<GetToDoTaskByIdQueryHandler> logger,
    IToDoTasksRepository toDoTasksRepository,
    IMapper mapper) 
    : IRequestHandler<GetToDoTaskByIdQuery, ToDoTaskDto>
{
    public async Task<ToDoTaskDto> Handle(GetToDoTaskByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting task by id");
        var task = await toDoTasksRepository.GetTaskByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(ToDoTaskDto), request.Id.ToString());

        var result = mapper.Map<ToDoTaskDto>(task);

        return result;
    }
}
