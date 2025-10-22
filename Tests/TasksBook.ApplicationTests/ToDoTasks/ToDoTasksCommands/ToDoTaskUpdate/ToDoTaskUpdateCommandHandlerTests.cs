using Xunit;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TasksBook.Domain.Repositories;
using TasksBook.Domain.Entities;

namespace TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTaskUpdate.Tests
{
    public class ToDoTaskUpdateCommandHandlerTests
    {
        private readonly Mock<ILogger<ToDoTaskUpdateCommandHandler>> _logger;
        private readonly Mock<IToDoTasksRepository> _toDoTaskRepository;
        private readonly Mock<IMapper> _mapper;

        public ToDoTaskUpdateCommandHandlerTests()
        {
            _logger = new Mock<ILogger<ToDoTaskUpdateCommandHandler>>();
            _toDoTaskRepository = new Mock<IToDoTasksRepository>();
            _mapper = new Mock<IMapper>();
        }
        [Fact()]
        public async Task Handle_FroValidUpdateCommand_TaskShouldBeUpdate()
        {
            //Arrange
            var command = new ToDoTaskUpdateCommand()
            {
                Id = Guid.NewGuid(),
                Name = "Update Test",
                Description = "Update Desc",
                ExpiresAt = DateTime.UtcNow.AddDays(1)               
            };

            var existTask = new ToDoTask()
            {
                Id = command.Id,
                Name = "Exist task",
                Description = "Exist desc",
                UpdatedAt = null,
                ExpiresAt = DateTime.UtcNow.AddDays(-1)
            };

            _toDoTaskRepository.Setup(c => c.GetTaskByIdAsync(command.Id)).ReturnsAsync(existTask);

            var handler = new ToDoTaskUpdateCommandHandler(
                _logger.Object,
                _toDoTaskRepository.Object,
                _mapper.Object);

            _mapper.Setup(x => x.Map(command, existTask)).Returns(existTask)
                .Callback<ToDoTaskUpdateCommand, ToDoTask>((req, task) =>
                {
                    task.Name = req.Name;
                    task.Description = req.Description;
                    task.ExpiresAt = req.ExpiresAt;
                });

            _toDoTaskRepository.Setup(s => s.UpdateTaskAsync(existTask)).Returns(Task.CompletedTask);
            //Act

            var result = await handler.Handle(command, CancellationToken.None);

            //Assert

            Assert.Equal(command.Name, existTask.Name);
            Assert.Equal(command.Description, existTask.Description);
            Assert.Equal(command.ExpiresAt, existTask.ExpiresAt);
            Assert.False(existTask.IsDone);
            Assert.NotNull(existTask.UpdatedAt);

            _toDoTaskRepository.Verify(a => a.GetTaskByIdAsync(command.Id), Times.Once());
            _toDoTaskRepository.Verify(c => c.UpdateTaskAsync(existTask), Times.Once());
        }
    }
}