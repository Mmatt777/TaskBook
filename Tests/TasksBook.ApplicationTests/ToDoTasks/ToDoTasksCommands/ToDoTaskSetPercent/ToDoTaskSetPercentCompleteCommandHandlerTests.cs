using Xunit;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TasksBook.Domain.Repositories;
using TasksBook.Domain.Entities;
using TasksBook.Domain.Exceptions;

namespace TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTaskSetPercent.Tests
{
    public class ToDoTaskSetPercentCompleteCommandHandlerTests
    {
        private readonly Mock<ILogger<ToDoTaskSetPercentCompleteCommandHandler>> _logger;
        private readonly Mock<IToDoTasksRepository> _toDoTaskRepository;
        private readonly Mock<IMapper> _mapper;

        public ToDoTaskSetPercentCompleteCommandHandlerTests()
        {
            _logger = new Mock<ILogger<ToDoTaskSetPercentCompleteCommandHandler>>();
            _toDoTaskRepository = new Mock<IToDoTasksRepository>();
            _mapper = new Mock<IMapper>();
        }
        [Fact()]
        public async Task Handle_ForExistingTaskAndSet100Percent_ShouldMarkAsDone()
        {
            // Arrange 
            var command = new ToDoTaskSetPercentCompleteCommand()
            {
                Id = Guid.NewGuid(),
                PercentComplete = 100,
                IsDone = true
            };

            var existingTask = new ToDoTask
            {
                Id = command.Id,
                PercentComplete = command.PercentComplete,
                IsDone = command.IsDone
            };

            _toDoTaskRepository.Setup(c => c.GetTaskByIdAsync(command.Id)).ReturnsAsync(existingTask);


            _mapper.Setup(c => c.Map(command, existingTask))
                .Callback<ToDoTaskSetPercentCompleteCommand, ToDoTask>((req, task) => 
                {
                    task.PercentComplete = req.PercentComplete;
                    task.CompletedAt = req.CompletedAt;
                });

            _toDoTaskRepository.Setup(x => x.UpdateTaskAsync(existingTask)).Returns(Task.CompletedTask);

            var handler = new ToDoTaskSetPercentCompleteCommandHandler(
                _logger.Object,
                _toDoTaskRepository.Object,
                _mapper.Object);

            // Act 

            var result = await handler.Handle(command, CancellationToken.None);

            // Assert

            Assert.Equal(command.Id, existingTask.Id);
            Assert.Equal(100, existingTask.PercentComplete);
            Assert.True(existingTask.IsDone);
            Assert.NotNull(existingTask.CompletedAt);

            _toDoTaskRepository.Verify(c => c.GetTaskByIdAsync(command.Id), Times.Once());
            _toDoTaskRepository.Verify(c => c.UpdateTaskAsync(existingTask), Times.Once());                      
        }

        [Fact()]
        public async Task Handle_ForExistingTaskAndSetLessThan100Percent_ShouldBeNotMarkAsDone()
        {
            // Arrange 
            var command = new ToDoTaskSetPercentCompleteCommand()
            {
                Id = Guid.NewGuid(),
                PercentComplete = 66,
            };

            var existingTask = new ToDoTask
            {
                Id = command.Id,
                PercentComplete = command.PercentComplete,
            };

            _toDoTaskRepository.Setup(c => c.GetTaskByIdAsync(command.Id)).ReturnsAsync(existingTask);


            _mapper.Setup(c => c.Map(command, existingTask));

            _toDoTaskRepository.Setup(x => x.UpdateTaskAsync(existingTask)).Returns(Task.CompletedTask);

            var handler = new ToDoTaskSetPercentCompleteCommandHandler(
                _logger.Object,
                _toDoTaskRepository.Object,
                _mapper.Object);

            // Act 

            var result = await handler.Handle(command, CancellationToken.None);

            // Assert

            Assert.Equal(command.Id, existingTask.Id);
            Assert.Equal(command.PercentComplete, existingTask.PercentComplete);
            Assert.False(existingTask.IsDone);
            Assert.Null(existingTask.CompletedAt);

            _toDoTaskRepository.Verify(c => c.GetTaskByIdAsync(command.Id), Times.Once());
            _toDoTaskRepository.Verify(c => c.UpdateTaskAsync(existingTask), Times.Once());
        }

        [Fact()]
        public async Task Handle_ForDoesNotExistTask_ShouldBeNotMarkAsDone()
        {
            // Arrange 
            var command = new ToDoTaskSetPercentCompleteCommand()
            {
                Id = Guid.NewGuid(),
                PercentComplete = 77
            };

            _toDoTaskRepository.Setup(c => c.GetTaskByIdAsync(command.Id)).ReturnsAsync((ToDoTask?)null);


            var handler = new ToDoTaskSetPercentCompleteCommandHandler(
                _logger.Object,
                _toDoTaskRepository.Object,
                _mapper.Object);

            // Act & Assert 
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));

            _toDoTaskRepository.Verify(c => c.GetTaskByIdAsync(command.Id), Times.Once());
            _toDoTaskRepository.Verify(c => c.UpdateTaskAsync(It.IsAny<ToDoTask>()), Times.Never());
        }
    }
}