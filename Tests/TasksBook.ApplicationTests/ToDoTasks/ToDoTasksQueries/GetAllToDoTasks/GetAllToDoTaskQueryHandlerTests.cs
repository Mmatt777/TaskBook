using Xunit;
using TasksBook.Application.ToDoTasks.ToDoTasksQueries.GetAllToDoTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Castle.Core.Logging;
using TasksBook.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TasksBook.Domain.Entities;
using TasksBook.Application.ToDoTasks.DTOS;

namespace TasksBook.Application.ToDoTasks.ToDoTasksQueries.GetAllToDoTasks.Tests
{
    public class GetAllToDoTaskQueryHandlerTests
    {
        private readonly Mock<ILogger<GetAllToDoTaskQueryHandler>> _logger;
        private readonly Mock<IToDoTasksRepository> _toDoTasksRepository;
        private readonly Mock<IMapper> _mapper;

        public GetAllToDoTaskQueryHandlerTests()
        {
            _logger = new Mock<ILogger<GetAllToDoTaskQueryHandler>>();
            _toDoTasksRepository = new Mock<IToDoTasksRepository>();
            _mapper = new Mock<IMapper>();
        }

        [Fact()]
        public async Task Handle_ForExistingTasks_ShouldReturnAllExistingTasks()
        {
            //Arrange
            var query = new GetAllToDoTaskQuery();

            var tasks = new List<ToDoTask>()
            {
                new() {Id = Guid.NewGuid(), Name = "Name1", Description = "Desc1" },
                new() {Id = Guid.NewGuid(), Name = "Name2", Description = "Desc2" }
            };

            var mappedTasks = new List<ToDoTaskDto>() 
            {
                new() {Id = tasks[0].Id, Name = tasks[0].Name, Description = tasks[0].Description},
                new() {Id = tasks[1].Id, Name = tasks[1].Name, Description = tasks[1].Description}
            };

            _toDoTasksRepository.Setup(c => c.GetAllAsync()).ReturnsAsync(tasks);

            _mapper.Setup(x => x.Map<IEnumerable<ToDoTaskDto>>(tasks)).Returns(mappedTasks);

            var handler = new GetAllToDoTaskQueryHandler(
                _logger.Object,
                _toDoTasksRepository.Object,
                _mapper.Object);
            //Act

            var result = await handler.Handle(query, CancellationToken.None);

            //Assert

            Assert.NotNull(result);
            Assert.Equal(mappedTasks.Count, result.Count());
            Assert.Equal(mappedTasks.First().Name, result.First().Name);
            Assert.Equal(mappedTasks.First().Description, result.First().Description);
            Assert.Equal(mappedTasks.Last().Name, result.Last().Name);
            Assert.Equal(mappedTasks.Last().Description, result.Last().Description);

            _toDoTasksRepository.Verify(c => c.GetAllAsync(), Times.Once());
        }

        [Fact()]
        public async Task Handle_ForDoesNotExistTasks_ShouldNotReturnTasks()
        {
            //Arrange
            var query = new GetAllToDoTaskQuery();

            var emptyList = new List<ToDoTask>();
            
            var emptyMappedList = new List<ToDoTaskDto>();
            
            _toDoTasksRepository.Setup(c => c.GetAllAsync()).ReturnsAsync(emptyList);

            _mapper.Setup(x => x.Map<IEnumerable<ToDoTaskDto>>(emptyList)).Returns(emptyMappedList);

            var handler = new GetAllToDoTaskQueryHandler(
                _logger.Object,
                _toDoTasksRepository.Object,
                _mapper.Object);
            //Act

            var result = await handler.Handle(query, CancellationToken.None);

            //Assert

            Assert.NotNull(result);
            Assert.Empty(result);

            _toDoTasksRepository.Verify(c => c.GetAllAsync(), Times.Once());
        }
    }
}