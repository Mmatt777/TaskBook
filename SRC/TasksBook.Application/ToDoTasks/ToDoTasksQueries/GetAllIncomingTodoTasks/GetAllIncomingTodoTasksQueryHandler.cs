using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TasksBook.Application.ToDoTasks.DTOS;
using TasksBook.Domain.Constants;
using TasksBook.Domain.Repositories;

namespace TasksBook.Application.ToDoTasks.ToDoTasksQueries.GetAllIncomingTodoTasks
{
    public class GetAllIncomingTodoTasksQueryHandler(ILogger<GetAllIncomingTodoTasksQueryHandler> logger,
        IToDoTasksRepository toDoTasksRepository,
        IMapper mapper) : IRequestHandler<GetAllIncomingTodoTasksQuery, IEnumerable<ToDoTaskDto>>
    {
        public async Task<IEnumerable<ToDoTaskDto>> Handle(GetAllIncomingTodoTasksQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting incoming task for today, tomorrow or week");
            var (start, end) = ResolveRange(request);
            var tasks = await toDoTasksRepository.GetIncomingTasks(start, end);

            var result = mapper.Map<IEnumerable<ToDoTaskDto>>(tasks);

            return result;
        }

        private static (DateTime start, DateTime end) ResolveRange(GetAllIncomingTodoTasksQuery req)
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            if (req.Period == IncomingPeriod.Tomorrow)
                return (tomorrow, tomorrow.AddDays(1));

            if (req.Period == IncomingPeriod.Week)
                return (today, today.AddDays(7));

            return (today, tomorrow);
        }
    }
}


