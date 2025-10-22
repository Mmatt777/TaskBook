using Xunit;
using Moq;
using TasksBook.Domain.Repositories;
using Microsoft.Extensions.Logging;
using TasksBook.Domain.Entities;
using System.Reflection.Metadata;
using FluentAssertions;

namespace TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTaskDelete.Tests
{
    public class ToDoTaskDeleteCommandHandlerTests
    {
        private readonly Mock<ILogger<ToDoTaskDeleteCommandHandler>> _logget;
        private readonly Mock<IToDoTasksRepository> _toDoTaskRepository;

        public ToDoTaskDeleteCommandHandlerTests()
        {
            _logget = new Mock<ILogger<ToDoTaskDeleteCommandHandler>>();
            _toDoTaskRepository = new Mock<IToDoTasksRepository>();
        }


        [Fact()]
        public async Task Handle_ForExistingTask_ShouldBeDeleteAndReturnTrue()
        {
            //Arrange
            var command = new ToDoTaskDeleteCommand(Guid.NewGuid());

            var existingTask = new ToDoTask() 
            { 
                Id = command.Id, 
                Name = "Test Task" 
            };

            _toDoTaskRepository.Setup(c => c.GetTaskByIdAsync(command.Id)).ReturnsAsync(existingTask);

            _toDoTaskRepository.Setup(x => x.DeleteTaskAsync(existingTask)).Returns(Task.CompletedTask);

            var hendler = new ToDoTaskDeleteCommandHandler(
                _logget.Object, 
                _toDoTaskRepository.Object);

            //Act 

            var result = await hendler.Handle(command, CancellationToken.None);

            //Assert 
            Assert.True(result);
            _toDoTaskRepository.Verify(c => c.GetTaskByIdAsync(command.Id), Times.Once());
            _toDoTaskRepository.Verify(x => x.DeleteTaskAsync(existingTask), Times.Once());

        }

        [Fact()]
        public async Task Handle_ForDoesNotExistTask_ShouldNotDeleteAndReturnFalse()
        {
            //Arrange
            var command = new ToDoTaskDeleteCommand(Guid.NewGuid());

            _toDoTaskRepository.Setup(c => c.GetTaskByIdAsync(command.Id)).ReturnsAsync((ToDoTask?)null);

            var handler = new ToDoTaskDeleteCommandHandler(
                _logget.Object,
                _toDoTaskRepository.Object);

            //Act 

            var result = await handler.Handle(command, CancellationToken.None);

            //Assert 
            Assert.False(result);

            _toDoTaskRepository.Verify(c => c.GetTaskByIdAsync(command.Id), Times.Once());
            _toDoTaskRepository.Verify(x => x.DeleteTaskAsync(It.IsAny<ToDoTask>()), Times.Never());

        }
    }
}