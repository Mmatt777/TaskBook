using AutoMapper;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TasksBook.Domain.Entities;
using TasksBook.Domain.Repositories;
using Xunit;


namespace TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTasksCreate.Tests;

public class ToDoTaskCreateCommandHandlerTests
{
    private readonly Mock<ILogger<ToDoTaskCreateCommandHandler>> _logger;
    private readonly Mock<IToDoTasksRepository> _ItoDoTaskRepository;
    private readonly Mock<IMapper> _mapper;

    public ToDoTaskCreateCommandHandlerTests()
    {
        _logger = new Mock<ILogger<ToDoTaskCreateCommandHandler>>();
        _ItoDoTaskRepository = new Mock<IToDoTasksRepository>();
        _mapper = new Mock<IMapper>();
    }

    [Fact()]
    public async Task Handle_ForValidCommand_ReturnsCreatedAndReturnGuidId()
    {
        // Arrange
        var commandTask = new ToDoTaskCreateCommand()
        {
            Name = "Test",
            Description = "Test desc",
            ExpiresAt = DateTime.UtcNow.AddDays(1)
        };

        var existingTask = new ToDoTask()
        {
            Name = commandTask.Name,
            Description = commandTask.Description,
            ExpiresAt = commandTask.ExpiresAt
        };

        var handler = new ToDoTaskCreateCommandHandler(
            _logger.Object,
            _ItoDoTaskRepository.Object,
            _mapper.Object);

        _mapper.Setup(c => c.Map<ToDoTask>(commandTask)).Returns(existingTask);

        var newTaskId = Guid.NewGuid();
        _ItoDoTaskRepository.Setup(c => c.CreateTaskAsync(It.IsAny<ToDoTask>())).ReturnsAsync(newTaskId);

        //Act 

        var result = await handler.Handle(commandTask, CancellationToken.None);

        //Assert

        Assert.Equal(newTaskId, result);
        _ItoDoTaskRepository.Verify(c => c.CreateTaskAsync(It.Is<ToDoTask>(
            z =>  z.IsDone == false)), Times.Once);

    }


}