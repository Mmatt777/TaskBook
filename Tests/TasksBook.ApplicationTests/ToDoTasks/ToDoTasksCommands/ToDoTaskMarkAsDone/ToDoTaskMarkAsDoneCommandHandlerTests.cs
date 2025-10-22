using Xunit;
using TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTaskMarkAsDone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTaskDelete;
using TasksBook.Domain.Repositories;
using TasksBook.Domain.Entities;
using AutoMapper;
using MediatR;
using TasksBook.Domain.Exceptions;

namespace TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTaskMarkAsDone.Tests
{
    public class ToDoTaskMarkAsDoneCommandHandlerTests
    {
        private readonly Mock<ILogger<ToDoTaskMarkAsDoneCommandHandler>> _logger;
        private readonly Mock<IToDoTasksRepository> _toDoTaskRepository;
        private readonly Mock<IMapper> _mapper;

        public ToDoTaskMarkAsDoneCommandHandlerTests()
        {
            _logger = new Mock<ILogger<ToDoTaskMarkAsDoneCommandHandler>>();
            _toDoTaskRepository = new Mock<IToDoTasksRepository>();
            _mapper = new Mock<IMapper>();
        }

        [Fact()]
        public async Task Handle_ForExistingTask_TaskShouldBeMarkAsDone()
        {
            // Arrange 
            var command = new ToDoTaskMarkAsDoneCommand()
            {
                Id = Guid.NewGuid()
            };

            var existingTask = new ToDoTask()
            {
                Id = command.Id,
                Name = "NameTest",
                Description = "Description",
                ExpiresAt = DateTime.UtcNow.AddDays(-1),
                PercentComplete = 50,
                IsDone = false,
                CompletedAt= null,
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            };

            _toDoTaskRepository.Setup(c => c.GetTaskByIdAsync(command.Id)).ReturnsAsync(existingTask);
   
            var handler = new ToDoTaskMarkAsDoneCommandHandler(
                _logger.Object,
                _toDoTaskRepository.Object,
                _mapper.Object
                );

            _mapper.Setup(c => c.Map(command, existingTask)).Returns(existingTask);

            _toDoTaskRepository.Setup(x => x.UpdateTaskAsync(existingTask)).Returns(Task.CompletedTask);
            // Act

            var result = await handler.Handle(command, CancellationToken.None);

            // Assert

            Assert.Equal(command.Id, existingTask.Id);
            Assert.Equal(100, existingTask.PercentComplete);
            Assert.True(existingTask.IsDone);
            Assert.NotNull(existingTask.CompletedAt);

            _toDoTaskRepository.Verify(c => c.GetTaskByIdAsync(command.Id), Times.Once);
            _toDoTaskRepository.Verify(x => x.UpdateTaskAsync(existingTask), Times.Once);
        }

        [Fact()]
        public async Task Handle_ForDoesNotExistTask_TaskShouldNotBeMarkAsDone()
        {
            // Arrange 
            var command = new ToDoTaskMarkAsDoneCommand()
            {
                Id = Guid.NewGuid()
            };


            _toDoTaskRepository.Setup(c => c.GetTaskByIdAsync(command.Id)).ReturnsAsync((ToDoTask?)null);

            var handler = new ToDoTaskMarkAsDoneCommandHandler(
                _logger.Object,
                _toDoTaskRepository.Object,
                _mapper.Object
                );

            // Act & Assert

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));


            _toDoTaskRepository.Verify(c => c.GetTaskByIdAsync(command.Id), Times.Once);
            _toDoTaskRepository.Verify(x => x.UpdateTaskAsync(It.IsAny<ToDoTask>()), Times.Never);
        }
    }
}