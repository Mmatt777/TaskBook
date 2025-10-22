using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TasksBook.Application.ToDoTasks.DTOS;
using TasksBook.Domain.Exceptions;
using TasksBook.Domain.Repositories;

namespace TasksBook.Application.ToDoTasks.ToDoTasksQueries.GetAllToDoTasks;

public class GetAllToDoTaskQueryHandler (ILogger<GetAllToDoTaskQueryHandler> logger,
    IToDoTasksRepository toDoTasksRepository,
    IMapper mapper)
    : IRequestHandler<GetAllToDoTaskQuery, IEnumerable<ToDoTaskDto>>
{
    public async Task<IEnumerable<ToDoTaskDto>> Handle(GetAllToDoTaskQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all todo taks");
        var tasks = await toDoTasksRepository.GetAllAsync();

        //var toDoTaskDto = tasks.Select(ToDoTaskDto.FromEntity);

        var toDoTaskDto = mapper.Map<IEnumerable<ToDoTaskDto>>(tasks);

        return toDoTaskDto;
    }
}
