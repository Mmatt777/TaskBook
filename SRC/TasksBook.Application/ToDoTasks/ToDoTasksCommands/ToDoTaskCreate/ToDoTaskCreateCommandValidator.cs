using FluentValidation;

namespace TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTasksCreate;
public class ToDoTaskCreateCommandValidator : AbstractValidator<ToDoTaskCreateCommand>
{
    public ToDoTaskCreateCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("Name of task is required.")
            .MinimumLength(3)
            .MaximumLength(100);

        RuleFor(c => c.Description)
            .NotEmpty()
            .WithMessage("Enter task description")
            .MinimumLength(10)
            .MaximumLength(100);

        RuleFor(c => c.ExpiresAt)
            .GreaterThan(_ => DateTime.UtcNow)
            .WithMessage("Date must be in the future.");
    }
}
