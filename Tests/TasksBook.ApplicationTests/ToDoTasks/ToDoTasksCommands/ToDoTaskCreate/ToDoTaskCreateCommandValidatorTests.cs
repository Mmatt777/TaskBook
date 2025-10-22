using Xunit;
using TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTasksCreate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;

namespace TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTasksCreate.Tests
{
    public class ToDoTaskCreateCommandValidatorTests
    {
        [Fact()]
        public void ToDoTaskCreateCommandValidator_ForValidCreateCommand_ShouldNotHaveValidationErrors()
        {
            //Arrange
            var command = new ToDoTaskCreateCommand()
            {
                Name = "TestTest",
                Description = "1234567890",
                ExpiresAt = DateTime.UtcNow.AddDays(1)
            };

            var validator = new ToDoTaskCreateCommandValidator();
            //Act

            var result = validator.TestValidate(command);

            //Assert

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact()]
        public void ToDoTaskCreateCommandValidator_ForInvalidCreateCommand_ShouldHaveValidationErrors()
        {
            //Arrange
            var command = new ToDoTaskCreateCommand()
            {
                Name = "T",
                Description = "1",
                ExpiresAt = DateTime.UtcNow
            };

            var validator = new ToDoTaskCreateCommandValidator();
            //Act

            var result = validator.TestValidate(command);

            //Assert

            result.ShouldHaveValidationErrors();
        }
    }
}