using Xunit;
using FluentValidation.TestHelper;

namespace TasksBook.Application.ToDoTasks.ToDoTasksCommands.ToDoTaskUpdate.Tests
{
    public class ToDoTaskUpdateCommandValidatorTests
    {
        [Fact()]
        public void ToDoTaskUpdateCommandValidatorTest_ForValidUpdateCommand_ShouldNotHaveValidationErrors()
        {
            //Arrange
            var command = new ToDoTaskUpdateCommand()
            {
                Name = "Valid",
                Description = "Valid desc",
                ExpiresAt = DateTime.UtcNow.AddDays(1)
            };

            var validator = new ToDoTaskUpdateCommandValidator();
            //Act

            var result = validator.TestValidate(command);

            //Assert

            result.ShouldNotHaveAnyValidationErrors();

        }

        [Fact()]
        public void ToDoTaskUpdateCommandValidatorTest_ForInvalidUpdateCommand_ShouldHaveValidationErrors()
        {
            //Arrange
            var command = new ToDoTaskUpdateCommand()
            {
                Name = "1",
                Description = "",
                ExpiresAt = DateTime.UtcNow
            };

            var validator = new ToDoTaskUpdateCommandValidator();
            //Act

            var result = validator.TestValidate(command);

            //Assert

            result.ShouldHaveValidationErrors();

        }
    }
}