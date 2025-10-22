using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTaskSetPercent
{
    public class ToDoTaskSetPercentCompleteCommandValidator : AbstractValidator<ToDoTaskSetPercentCompleteCommand>
    {
        public ToDoTaskSetPercentCompleteCommandValidator()
        {
            RuleFor(c => c.PercentComplete)
                .InclusiveBetween(0, 100)
                .WithMessage("The value must be between 0 and 100!");
        }
    }
}
