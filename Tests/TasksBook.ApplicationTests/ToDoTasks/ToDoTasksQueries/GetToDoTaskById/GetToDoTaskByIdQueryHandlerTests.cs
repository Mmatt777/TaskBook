using Xunit;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TasksBook.Domain.Repositories;
using TasksBook.Domain.Entities;
using TasksBook.Application.ToDoTasks.DTOS;
using TasksBook.Domain.Exceptions;

namespace TasksBook.Application.ToDoTasks.ToDoTasksQueries.GetToDoTaskById.Tests
{
    public class GetToDoTaskByIdQueryHandlerTests
    {
        private readonly Mock<ILogger<GetToDoTaskByIdQueryHandler>> _logger;
        private readonly Mock<IToDoTasksRepository> _toDoTasksRepository;
        private readonly Mock<IMapper> _mapper;

        public GetToDoTaskByIdQueryHandlerTests()
        {
            _logger = new Mock<ILogger<GetToDoTaskByIdQueryHandler>>();
            _toDoTasksRepository = new Mock<IToDoTasksRepository>();
            _mapper = new Mock<IMapper>();
        }

        [Fact()]
        public async Task Handle_FroExistTask_ShouldReturnTask()
        {
            //Arrange
            var id = Guid.NewGuid();
            var query = new GetToDoTaskByIdQuery(id);

            var existTask = new ToDoTask()
            {
                Id = id,
                Name = "GetById",
                Description = "Desc",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                IsDone = false,
                PercentComplete = 20,
                ExpiresAt = DateTime.UtcNow
            };

            var mappedTask = new ToDoTaskDto()
            {
                Id = existTask.Id,
                Name = existTask.Name,
                Description = existTask.Description,
                CreatedAt = existTask.CreatedAt,
                IsDone = existTask.IsDone,
                PercentComplete = existTask.PercentComplete,
                ExpiresAt = existTask.ExpiresAt
            };

            _toDoTasksRepository.Setup(c => c.GetTaskByIdAsync(query.Id)).ReturnsAsync(existTask);

            _mapper.Setup(m => m.Map<ToDoTaskDto>(existTask)).Returns(mappedTask);

            var handler = new GetToDoTaskByIdQueryHandler(
                _logger.Object,
                _toDoTasksRepository.Object,
                _mapper.Object);

            //Act 

            var result = await handler.Handle(query, CancellationToken.None);

            //Asserst 

            Assert.NotNull(result);
            Assert.Equal(mappedTask.Id, result.Id);
            Assert.Equal(mappedTask.Name, result.Name);
            Assert.Equal(mappedTask.Description, result.Description);
            Assert.Equal(mappedTask.CreatedAt, result.CreatedAt);
            Assert.Equal(mappedTask.IsDone, result.IsDone);
            Assert.Equal(mappedTask.PercentComplete, result.PercentComplete);
            Assert.Equal(mappedTask.ExpiresAt, result.ExpiresAt);
            Assert.False(result.IsDone);

            _toDoTasksRepository.Verify(c => c.GetTaskByIdAsync(query.Id), Times.Once());
        }

        [Fact()]
        public async Task Handle_FroDoesNotExistTask_ShouldReturnNotFoundException()
        {
            //Arrange
            var id = Guid.NewGuid();
            var query = new GetToDoTaskByIdQuery(id);

            _toDoTasksRepository.Setup(c => c.GetTaskByIdAsync(query.Id)).ReturnsAsync((ToDoTask?)null);


            var handler = new GetToDoTaskByIdQueryHandler(
                _logger.Object,
                _toDoTasksRepository.Object,
                _mapper.Object);


            //Asserst & Act

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));


            _toDoTasksRepository.Verify(c => c.GetTaskByIdAsync(query.Id), Times.Once());
        }
    }
}