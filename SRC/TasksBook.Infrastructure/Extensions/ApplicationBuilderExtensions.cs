using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TasksBook.Infrastructure.Persistens;
using TasksBook.Infrastructure.Seeders;

namespace TasksBook.Infrastructure.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task MigrationApply(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<TasksBookDbContext>();
        await dbContext.Database.MigrateAsync();

        var seeder = scope.ServiceProvider.GetRequiredService<ITasksBookSeeder>();
        await seeder.Seed();
    }
}
