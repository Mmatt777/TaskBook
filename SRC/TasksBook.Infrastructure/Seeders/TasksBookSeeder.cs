using System.Globalization;
using TasksBook.Domain.Entities;
using TasksBook.Infrastructure.Persistens;

namespace TasksBook.Infrastructure.Seeders;

public class TasksBookSeeder(TasksBookDbContext dbContext) : ITasksBookSeeder
{
    public async Task Seed()
    {
        if (await dbContext.Database.CanConnectAsync())
        {
            if (!dbContext.Tasks.Any())
            {
                var todoTaks = GetToDoTasks();
                dbContext.Tasks.AddRange(todoTaks);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    private IEnumerable<ToDoTask> GetToDoTasks()
    {
        List<ToDoTask> tasks = [
            new()
            {
                Name = "Call to customer",
                Description = "Talk about something",
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                PercentComplete = 0,
                IsDone = false,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "Prepare meeting agenda",
                Description = "Draft the agenda for Monday's project meeting",
                ExpiresAt = DateTime.UtcNow.AddDays(2),
                PercentComplete = 0,
                IsDone = false,
                CreatedAt = DateTime.UtcNow.AddDays(1)
            },
            new()
            {
                Name = "Send invoices",
                Description = "Email invoices to clients for the current month",
                ExpiresAt = DateTime.UtcNow.AddDays(3),
                PercentComplete = 0,
                IsDone = false,
                CreatedAt = DateTime.UtcNow.AddDays(2)
            },
            new()
            {
                Name = "Code review",
                Description = "Review pull requests from development team",
                ExpiresAt = DateTime.UtcNow.AddDays(4),
                PercentComplete = 0,
                IsDone = false,
                CreatedAt = DateTime.UtcNow.AddDays(3)

            },
            new()
            {
                Name = "Database backup",
                Description = "Perform a full backup of the production database",
                ExpiresAt = DateTime.UtcNow.AddDays(6),
                PercentComplete = 0,
                IsDone = false,
                CreatedAt = DateTime.UtcNow.AddDays(5)
            },
            new()
            {
                Name = "Write documentation",
                Description = "Update API documentation with recent changes",
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                PercentComplete = 0,
                IsDone = false,
                CreatedAt = DateTime.UtcNow.AddDays(6)
            },
            new()
            {
                Name = "Test deployment pipeline",
                Description = "Ensure CI/CD pipeline works correctly after updates",
                ExpiresAt = DateTime.UtcNow.AddDays(8),
                PercentComplete = 0,
                IsDone = false,
                CreatedAt = DateTime.UtcNow.AddDays(7)
            },
            new()
            {
                Name = "Team feedback session",
                Description = "Conduct a team meeting to gather sprint feedback",
                ExpiresAt = DateTime.UtcNow.AddDays(9),
                PercentComplete = 0,
                IsDone = false,
                CreatedAt = DateTime.UtcNow.AddDays(8)
            },
            new()
            {
                Name = "Prepare release notes",
                Description = "Write release notes for the upcoming version",
                ExpiresAt = DateTime.UtcNow.AddDays(10),
                PercentComplete = 0,
                IsDone = false,
                CreatedAt = DateTime.UtcNow.AddDays(9)
            },
            new()
            {
                Name = "Design review",
                Description = "Review UI changes with the design team",
                ExpiresAt = DateTime.UtcNow.AddDays(11),
                PercentComplete = 0,
                IsDone = false,
                CreatedAt = DateTime.UtcNow.AddDays(10)
            },
            new()
            {
                Name = "Customer feedback analysis",
                Description = "Analyze results from the latest customer survey",
                ExpiresAt = DateTime.UtcNow.AddDays(12),
                PercentComplete = 0,
                IsDone = false,
                CreatedAt = DateTime.UtcNow.AddDays(11)
            },
            new()
            {
                Name = "Plan next sprint",
                Description = "Create sprint goals and assign tasks for next iteration",
                ExpiresAt = DateTime.UtcNow.AddDays(13),
                PercentComplete = 0,
                IsDone = false,
                CreatedAt = DateTime.UtcNow.AddDays(12)
            }
        ];

        return tasks;
    }
}
