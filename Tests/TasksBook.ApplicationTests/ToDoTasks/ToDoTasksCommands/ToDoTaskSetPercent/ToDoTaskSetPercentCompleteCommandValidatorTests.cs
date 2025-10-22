using Xunit;
using TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTaskSetPercent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;

namespace TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTaskSetPercent.Tests
{
    public class ToDoTaskSetPercentCompleteCommandValidatorTests
    {
        [Fact()]
        public void ToDoTaskSetPercentCompleteCommandValidator_ForValidSetPercent_ShouldNotHaveValidationErrors()
        {
            //Arrange
            var command = new ToDoTaskSetPercentCompleteCommand()
            {
                PercentComplete = 55
            };

            var validator = new ToDoTaskSetPercentCompleteCommandValidator();
            //Act

            var result = validator.TestValidate(command);

            //Assert

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact()]
        public void ToDoTaskSetPercentCompleteCommandValidator_ForInvalidSetPercent_ShouldHaveValidationErrors()
        {
            //Arrange
            var command = new ToDoTaskSetPercentCompleteCommand()
            {
                PercentComplete = 555
            };

            var validator = new ToDoTaskSetPercentCompleteCommandValidator();
            //Act

            var result = validator.TestValidate(command);

            //Assert

            result.ShouldHaveValidationErrors();
        }
    }
}