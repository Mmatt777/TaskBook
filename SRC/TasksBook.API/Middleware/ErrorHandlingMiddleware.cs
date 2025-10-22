
using TasksBook.Domain.Exceptions;

namespace TasksBook.API.Middleware
{
    public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next) 
        {
            try
            {
                await next.Invoke(context);
            }
            catch(NotFoundException notFoundEx)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(notFoundEx.Message);
                logger.LogError(notFoundEx.Message);

            }
            catch(Exception ex)
            {
                logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something went wrong!");
            }
        }
    }
}
