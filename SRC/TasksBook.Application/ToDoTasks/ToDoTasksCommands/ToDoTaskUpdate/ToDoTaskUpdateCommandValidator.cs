using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTaskUpdate
{
    public class ToDoTaskUpdateCommandValidator : AbstractValidator<ToDoTaskUpdateCommand>
    {
        public ToDoTaskUpdateCommandValidator()
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
}
