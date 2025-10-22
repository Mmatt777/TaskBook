using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTasksCreate;
using Xunit;

namespace TasksBook.API.Controllers.Tests
{
    public class ToDoTasksControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ToDoTasksControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact()]
        public async Task GetAll_FroValidatedRequest_Returns200OK()
        {
            //Arrange

            var client = _factory.CreateClient();

            //Act

            var result = await client.GetAsync("api/todotasks");

            //Assert

            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);


        }
        
        [Fact()]
        public async Task GetAll_FroInvalidatedRequest_Returns404NotFound()
        {
            //Arrange

            var client = _factory.CreateClient();

            //Act

            var result = await client.GetAsync("api/todotasksss");

            //Assert

            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);


        }
        
        [Fact()]
        public async Task GetAll_FroInvalidatedRequest_Returns400BadRequest()
        {
            //Arrange

            var client = _factory.CreateClient()    ;

            //Act

            var result = await client.GetAsync("api/todotasks/xxxxx");

            //Assert

            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);


        }

        [Fact()]
        public async Task CreateNewTask_ForValidRequest_Returns201Created()
        {
            //Arrange

            var client = _factory.CreateClient();

            var command = new ToDoTaskCreateCommand()
            {
                Name = "Test Task",
                Description = "Integration test task",
                ExpiresAt = DateTime.UtcNow.AddDays(1)
            };

            //Act

            var result = await client.PostAsJsonAsync("api/todotasks", command);

            //Assert

            result.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        [Fact()]
        public async Task GetTaskById_ForExistingTask_Returns200OK()
        {
            //Arrange

            var client = _factory.CreateClient();

            var command = new ToDoTaskCreateCommand()
            {
                Name = "Existing Task",
                Description = "Should be found",
                ExpiresAt = DateTime.UtcNow.AddDays(2)
            };

            var createResponse = await client.PostAsJsonAsync("api/todotasks", command);
            var id = await createResponse.Content.ReadFromJsonAsync<Guid>();

            //Act

            var result = await client.GetAsync($"api/todotasks/{id}");

            //Assert

            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact()]
        public async Task GetTaskById_ForNotExistingTask_Returns404NotFound()
        {
            //Arrange

            var client = _factory.CreateClient();
            var id = Guid.NewGuid();

            //Act

            var result = await client.GetAsync($"api/todotasks/{id}");

            //Assert

            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

    }
}