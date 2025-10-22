using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TasksBook.Application.ToDoTasks.DTOS;
using TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTaskDelete;
using TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTaskMarkAsDone;
using TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTasksCreate;
using TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTaskSetPercent;
using TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTaskUpdate;
using TasksBook.Application.ToDoTasks.ToDoTasksQueries.GetAllIncomingTodoTasks;
using TasksBook.Application.ToDoTasks.ToDoTasksQueries.GetAllToDoTasks;
using TasksBook.Application.ToDoTasks.ToDoTasksQueries.GetToDoTaskById;
using TasksBook.Domain.Constants;

namespace TasksBook.API.Controllers
{
    [ApiController]
    [Route("api/todotasks")]
    public class ToDoTasksController(IMediator mediator) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoTaskDto>>> GetAllToDoTasks()
        {
            var tasks = await mediator.Send(new GetAllToDoTaskQuery());

            return Ok(tasks);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ToDoTaskDto>> GetToDoTaskById([FromRoute] Guid id)
        {
            var task = await mediator.Send(new GetToDoTaskByIdQuery(id));

            return Ok(task);
        }

        [HttpGet]
        [Route("Incoming")]
        public async Task<ActionResult<IEnumerable<ToDoTaskDto>>> GetAllIncomingToDoTasks([FromQuery] IncomingPeriod period)
        {
            var tasks = await mediator.Send(new GetAllIncomingTodoTasksQuery(period));

            return Ok(tasks);
        }


        [HttpPost]
        public async Task<IActionResult> CreateNewTask([FromBody] ToDoTaskCreateCommand toDoTaskCreateCommand)
        {
            var id = await mediator.Send(toDoTaskCreateCommand);

            return CreatedAtAction(nameof(GetToDoTaskById), new { id }, id);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteTask([FromRoute] Guid id)
        {
            var IsDeleted = await mediator.Send(new ToDoTaskDeleteCommand(id));

            if(IsDeleted)
            return NoContent();

            return NotFound();
        }

        [HttpPatch]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateTask([FromRoute] Guid id,[FromBody]ToDoTaskUpdateCommand toDoTaskUpdateCommand)
        {
            toDoTaskUpdateCommand.Id = id;
            var task = await mediator.Send(toDoTaskUpdateCommand);

            return Ok(task);
        }

        [HttpPatch]
        [Route("mark/{id}")]
        public async Task<IActionResult> MarkAsDoneTask([FromRoute] Guid id, [FromBody] ToDoTaskMarkAsDoneCommand toDoTaskMarkAsDoneCommand)
        {
            toDoTaskMarkAsDoneCommand.Id = id;
            var task = await mediator.Send(toDoTaskMarkAsDoneCommand);

            return Ok(task);
        }
        
        [HttpPatch]
        [Route("setPercentComplete/{id}")]
        public async Task<IActionResult> SetPercentCompleteTask([FromRoute] Guid id, [FromBody] ToDoTaskSetPercentCompleteCommand toDoTaskSetPercentCompleteCommand)
        {
            toDoTaskSetPercentCompleteCommand.Id = id;
            var task = await mediator.Send(toDoTaskSetPercentCompleteCommand);

            return Ok(task);
        }

    }
}
