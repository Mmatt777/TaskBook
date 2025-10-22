using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using TasksBook.Domain.Entities;
using TasksBook.Domain.Exceptions;
using Xunit;


namespace TasksBook.API.Middleware.Tests
{
    public class ErrorHandlingMiddlewareTests
    {
        private readonly Mock<ILogger<ErrorHandlingMiddleware>> _logger;
        private readonly Mock<RequestDelegate> _reqDelegate;

        public ErrorHandlingMiddlewareTests()
        {
            _logger = new Mock<ILogger<ErrorHandlingMiddleware>>();
            _reqDelegate = new Mock<RequestDelegate>();
        }

        [Fact()]
        public async Task InvokeAsync_WhenNoExceptionThrow_ShouldCallNextDelegate()
        {
            // Arrange 
            var middleware = new ErrorHandlingMiddleware(_logger.Object);
            var context = new DefaultHttpContext();

            // Act 

            await middleware.InvokeAsync(context, _reqDelegate.Object);

            // Assert 

            _reqDelegate.Verify(n => n.Invoke(context), Times.Once());
        }

        [Fact()]
        public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCode404()
        {
            // Arrange 
            var middleware = new ErrorHandlingMiddleware(_logger.Object);
            var context = new DefaultHttpContext();
            var notFoundExcetpion = new NotFoundException(nameof(ToDoTask), Guid.NewGuid().ToString());

            // Act 

            await middleware.InvokeAsync(context, _ => throw notFoundExcetpion);

            // Assert 

            context.Response.StatusCode.Should().Be(404);
        }

        [Fact()]
        public async Task InvokeAsync_WhenExceptionThrown_ShouldSetStatusCode500()
        {
            // Arrange 
            var middleware = new ErrorHandlingMiddleware(_logger.Object);
            var context = new DefaultHttpContext();
            var excetpion = new Exception();

            // Act 

            await middleware.InvokeAsync(context, _ => throw excetpion);

            // Assert 

            context.Response.StatusCode.Should().Be(500);
        }
    }
}