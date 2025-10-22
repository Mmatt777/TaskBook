using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TasksBook.Domain.Repositories;
using TasksBook.Infrastructure.Persistens;
using TasksBook.Infrastructure.Repositories;
using TasksBook.Infrastructure.Seeders;

namespace TasksBook.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var postgresqlConnString = configuration.GetConnectionString("TasksBookDb");
        services.AddDbContext<TasksBookDbContext>(options => options.UseNpgsql(postgresqlConnString));

        services.AddScoped<ITasksBookSeeder, TasksBookSeeder>();
        services.AddScoped<IToDoTasksRepository, ToDoTasksRepository>();
    }

}
