using Application.Features.TaskItem.Commands.Delete;
using Domain.Entities.Dts;
using Domain.Exceptions;
using Moq;
using Tests.Unit.Core;
using Tests.Unit.Helpers;

namespace Tests.Unit.Handlers.Commands
{
    /// <summary>
    /// Unit tests for <see cref="DeleteTaskItemCommandHandler"/>.
    /// Ensures that deleting a <see cref="TaskItem"/> behaves as expected.
    /// </summary>
    public class DeleteTaskItemCommandHandlerTest : HandlerTestBase
    {
        /// <summary>
        /// The handler for the deleting a task.
        /// </summary>
        private readonly DeleteTaskItemCommandHandler _handler;

        /// <summary>
        /// Verifies that a <see cref="TaskItem"/> is successfully deleted.
        /// </summary>
        [Fact]
        public async Task Handle_ShouldDeleteTaskItem_WhenTaskItemExists()
        {
            // Arrange
            var taskItem = TaskItemTestHelper.GetTaskItem();
            var command = new DeleteTaskItemCommand { Id = taskItem.Id };

            TaskItemRepositoryMock
                .Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(taskItem);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            TaskItemRepositoryMock.Verify(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
            TaskItemRepositoryMock.Verify(r => r.Remove(taskItem), Times.Once);
        }

        /// <summary>
        /// Verifies that an exception is thrown when attempting to delete
        /// a <see cref="TaskItem"/> that does not exist.
        /// </summary>
        [Fact]
        public async Task Handle_ShouldThrowException_WhenTaskItemDoesNotExist()
        {
            // Arrange
            var command = new DeleteTaskItemCommand { Id = 999 };

            TaskItemRepositoryMock
                .Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            {
                await _handler.Handle(command, CancellationToken.None);
            });

            Assert.IsType<EntityNotFoundException>(ex);

            TaskItemRepositoryMock.Verify(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
            TaskItemRepositoryMock.Verify(r => r.Remove(It.IsAny<TaskItem>()), Times.Never);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteTaskItemCommandHandlerTest"/> class.
        /// </summary>
        public DeleteTaskItemCommandHandlerTest()
        {
            _handler = new (TaskItemRepositoryMock.Object);
        }
    }
}
