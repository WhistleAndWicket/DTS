using Application.Features.TaskItem.Commands.Create;
using Domain.Entities.Dts;
using DTOs.Shared.Enums;
using Moq;
using Tests.Unit.Core;
using Tests.Unit.Helpers;

namespace Tests.Unit.Handlers.Commands
{
    /// <summary>
    /// Unit tests for <see cref="CreateTaskItemCommandHandler"/>.
    /// Ensures that creating a new <see cref="TaskItem"/> behaves as expected.
    /// </summary>
    public class CreateTaskItemCommandHandlerTest : HandlerTestBase
    {
        /// <summary>
        /// The handler for the creating a task.
        /// </summary>
        private readonly CreateTaskItemCommandHandler _handler;

        /// <summary>
        /// Verifies that a new <see cref="TaskItem"/> is successfully created
        /// </summary>
        [Fact]
        public async Task Handle_ShouldCreateTaskItem_WhenTitleIsUnique()
        {
            // Arrange
            var taskItem = TaskItemTestHelper.GetTaskItem();

            var command = new CreateTaskItemCommand
            {
                Title = taskItem.Title,
                Description = taskItem.Description,
                DueDate = taskItem.DueDate
            };

            TaskItemRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(taskItem);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.Equal(command.Title, result.Title);
            Assert.Equal(command.Description, result.Description);
            Assert.Equal(TaskItemStatus.Created, result.Status);

            TaskItemRepositoryMock.Verify(r => r.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTaskItemCommandHandlerTest"/> class.
        /// </summary>
        public CreateTaskItemCommandHandlerTest()
        {
            _handler = new(TaskItemRepositoryMock.Object, UnitOfWorkMock.Object);
        }
    }
}
