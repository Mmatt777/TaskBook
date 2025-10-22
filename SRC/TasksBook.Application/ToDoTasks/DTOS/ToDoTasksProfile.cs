using AutoMapper;
using TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTaskMarkAsDone;
using TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTasksCreate;
using TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTaskSetPercent;
using TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTaskUpdate;
using TasksBook.Domain.Entities;

namespace TasksBook.Application.ToDoTasks.DTOS;

public class ToDoTasksProfile : Profile
{
    public ToDoTasksProfile()
    {
        CreateMap<ToDoTask, ToDoTaskDto>();
        CreateMap<ToDoTaskCreateCommand, ToDoTask>();
        CreateMap<ToDoTaskUpdateCommand, ToDoTask>();
        CreateMap<ToDoTaskMarkAsDoneCommand, ToDoTask>();
        CreateMap<ToDoTaskSetPercentCompleteCommand, ToDoTask>();
    }
}
