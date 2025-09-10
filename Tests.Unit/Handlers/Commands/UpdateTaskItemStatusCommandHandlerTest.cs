using Application.Features.TaskItem.Commands.Update;
using Domain.Exceptions;
using DTOs.Shared.Enums;
using Moq;
using Tests.Unit.Core;
using Tests.Unit.Helpers;

namespace Tests.Unit.Handlers.Commands
{
    /// <summary>
    /// Unit tests for <see cref="UpdateTaskItemStatusCommandHandler"/>.
    /// Ensures that updating the status of a task item behaves as expected.
    /// </summary>
    public class UpdateTaskItemStatusCommandHandlerTest : HandlerTestBase
    {
        /// <summary>
        /// The handler for updating the status of a task item.
        /// </summary>
        private readonly UpdateTaskItemStatusCommandHandler _handler;

        /// <summary>
        /// Verifies that the status of a TaskItem is successfully updated.
        /// </summary>
        [Fact]
        public async Task Handle_ShouldUpdateStatus_WhenTaskItemExists()
        {
            // Arrange
            var taskItem = TaskItemTestHelper.GetTaskItem();
            var command = new UpdateTaskItemStatusCommand
            {
                Id = taskItem.Id,
                Status = TaskItemStatus.Completed
            };

            TaskItemRepositoryMock
                .Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(taskItem);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(TaskItemStatus.Completed, result.Status);
            TaskItemRepositoryMock.Verify(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Verifies that an exception is thrown when attempting to update
        /// the status of a TaskItem that does not exist.
        /// </summary>
        [Fact]
        public async Task Handle_ShouldThrowException_WhenTaskItemDoesNotExist()
        {
            // Arrange.
            var command = new UpdateTaskItemStatusCommand
            {
                Id = 123,
                Status = TaskItemStatus.Completed
            };

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
            UnitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateTaskItemStatusCommandHandlerTest"/> class.
        /// </summary>
        public UpdateTaskItemStatusCommandHandlerTest()
        {
            _handler = new(TaskItemRepositoryMock.Object);
        }
    }
}
