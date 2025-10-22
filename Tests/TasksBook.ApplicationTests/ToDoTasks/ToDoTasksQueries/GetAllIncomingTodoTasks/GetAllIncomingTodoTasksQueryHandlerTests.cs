using Xunit;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TasksBook.Domain.Repositories;
using TasksBook.Application.ToDoTasks.DTOS;
using TasksBook.Domain.Constants;
using TasksBook.Domain.Entities;

namespace TasksBook.Application.ToDoTasks.ToDoTasksQueries.GetAllIncomingTodoTasks.Tests
{
    public class GetAllIncomingTodoTasksQueryHandlerTests
    {
        private readonly Mock<ILogger<GetAllIncomingTodoTasksQueryHandler>> _logger;
        private readonly Mock<IToDoTasksRepository> _toDoTasksRepository;
        private readonly Mock<IMapper> _mapper;

        public GetAllIncomingTodoTasksQueryHandlerTests()
        {
            _logger = new Mock<ILogger<GetAllIncomingTodoTasksQueryHandler>>();
            _toDoTasksRepository = new Mock<IToDoTasksRepository>();
            _mapper = new Mock<IMapper>();
        }

        [Fact()]
        public async Task Handle_ForTodayPeriod_ShouldReturnMappedTasks()
        {
            //Arrange 

            var query = new GetAllIncomingTodoTasksQuery(IncomingPeriod.Today);

            var now = DateTime.UtcNow;
            var today = now.Date;
            var tomorrow = today.AddDays(1);

            var existTasks = new List<ToDoTask>()
            {
                new() {Id = Guid.NewGuid(), Name = "Test1", ExpiresAt = DateTime.UtcNow },

                new() {Id = Guid.NewGuid(), Name = "Test2", ExpiresAt = DateTime.UtcNow }               
            };

            var mappedTasks = new List<ToDoTaskDto>()
            {
                new() {Id = existTasks[0].Id, Name = existTasks[0].Name , ExpiresAt = existTasks[0].ExpiresAt},

                new() {Id = existTasks[1].Id, Name = existTasks[1].Name , ExpiresAt = existTasks[1].ExpiresAt}
            };

            _toDoTasksRepository.Setup(c => c.GetIncomingTasks(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(existTasks);

            _mapper.Setup(m => m.Map<IEnumerable<ToDoTaskDto>>(existTasks))
                .Returns(mappedTasks);

            var handler = new GetAllIncomingTodoTasksQueryHandler(
                _logger.Object,
                _toDoTasksRepository.Object,
                _mapper.Object);

            // Act 

            var result = await handler.Handle(query, CancellationToken.None);

            // Assert 
            Assert.NotNull(result);
            Assert.Equal(mappedTasks.Count, result.Count());
            Assert.Equal(mappedTasks.First().Id, result.First().Id);
            Assert.Equal(mappedTasks.First().Name, result.First().Name);
            Assert.Equal(mappedTasks.First().ExpiresAt, result.First().ExpiresAt);
            Assert.Equal(mappedTasks.Last().Id, result.Last().Id);
            Assert.Equal(mappedTasks.Last().Name, result.Last().Name);
            Assert.Equal(mappedTasks.Last().ExpiresAt, result.Last().ExpiresAt);

            _toDoTasksRepository.Verify(c => c.GetIncomingTasks(It.Is<DateTime>(d => d.Date == today),
                It.Is<DateTime>(d => d.Date == tomorrow)), Times.Once());
        }

        [Fact()]
        public async Task Handle_ForTomorrowPeriod_ShouldReturnMappedTasks()
        {
            //Arrange 

            var query = new GetAllIncomingTodoTasksQuery(IncomingPeriod.Tomorrow);

            var now = DateTime.UtcNow;
            var tomorrow = now.Date.AddDays(1);
            var dayAfterTomorrow = tomorrow.AddDays(1);

            var existTasks = new List<ToDoTask>()
            {
                new() {Id = Guid.NewGuid(), Name = "Test1", ExpiresAt = DateTime.UtcNow },

                new() {Id = Guid.NewGuid(), Name = "Test2", ExpiresAt = DateTime.UtcNow }
            };

            var mappedTasks = new List<ToDoTaskDto>()
            {
                new() {Id = existTasks[0].Id, Name = existTasks[0].Name , ExpiresAt = existTasks[0].ExpiresAt},

                new() {Id = existTasks[1].Id, Name = existTasks[1].Name , ExpiresAt = existTasks[1].ExpiresAt}
            };

            _toDoTasksRepository.Setup(c => c.GetIncomingTasks(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(existTasks);

            _mapper.Setup(m => m.Map<IEnumerable<ToDoTaskDto>>(existTasks))
                .Returns(mappedTasks);

            var handler = new GetAllIncomingTodoTasksQueryHandler(
                _logger.Object,
                _toDoTasksRepository.Object,
                _mapper.Object);

            // Act 

            var result = await handler.Handle(query, CancellationToken.None);

            // Assert 
            Assert.NotNull(result);
            Assert.Equal(mappedTasks.Count, result.Count());
            Assert.Equal(mappedTasks.First().Id, result.First().Id);
            Assert.Equal(mappedTasks.First().Name, result.First().Name);
            Assert.Equal(mappedTasks.First().ExpiresAt, result.First().ExpiresAt);
            Assert.Equal(mappedTasks.Last().Id, result.Last().Id);
            Assert.Equal(mappedTasks.Last().Name, result.Last().Name);
            Assert.Equal(mappedTasks.Last().ExpiresAt, result.Last().ExpiresAt);

            _toDoTasksRepository.Verify(c => c.GetIncomingTasks(It.Is<DateTime>(d => d.Date == tomorrow),
                It.Is<DateTime>(d => d.Date == dayAfterTomorrow)), Times.Once());
        }

        [Fact()]
        public async Task Handle_ForWeekPeriod_ShouldReturnMappedTasks()
        {
            //Arrange 

            var query = new GetAllIncomingTodoTasksQuery(IncomingPeriod.Week);

            var now = DateTime.UtcNow;
            var today = now.Date;
            var week = today.AddDays(7);

            var existTasks = new List<ToDoTask>()
            {
                new() {Id = Guid.NewGuid(), Name = "Test1", ExpiresAt = DateTime.UtcNow },

                new() {Id = Guid.NewGuid(), Name = "Test2", ExpiresAt = DateTime.UtcNow }
            };

            var mappedTasks = new List<ToDoTaskDto>()
            {
                new() {Id = existTasks[0].Id, Name = existTasks[0].Name , ExpiresAt = existTasks[0].ExpiresAt},

                new() {Id = existTasks[1].Id, Name = existTasks[1].Name , ExpiresAt = existTasks[1].ExpiresAt}
            };

            _toDoTasksRepository.Setup(c => c.GetIncomingTasks(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(existTasks);

            _mapper.Setup(m => m.Map<IEnumerable<ToDoTaskDto>>(existTasks))
                .Returns(mappedTasks);

            var handler = new GetAllIncomingTodoTasksQueryHandler(
                _logger.Object,
                _toDoTasksRepository.Object,
                _mapper.Object);

            // Act 

            var result = await handler.Handle(query, CancellationToken.None);

            // Assert 
            Assert.NotNull(result);
            Assert.Equal(mappedTasks.Count, result.Count());
            Assert.Equal(mappedTasks.First().Id, result.First().Id);
            Assert.Equal(mappedTasks.First().Name, result.First().Name);
            Assert.Equal(mappedTasks.First().ExpiresAt, result.First().ExpiresAt);
            Assert.Equal(mappedTasks.Last().Id, result.Last().Id);
            Assert.Equal(mappedTasks.Last().Name, result.Last().Name);
            Assert.Equal(mappedTasks.Last().ExpiresAt, result.Last().ExpiresAt);

            _toDoTasksRepository.Verify(c => c.GetIncomingTasks(It.Is<DateTime>(d => d.Date == today),
                It.Is<DateTime>(d => d.Date == week)), Times.Once());
        }
    }
}