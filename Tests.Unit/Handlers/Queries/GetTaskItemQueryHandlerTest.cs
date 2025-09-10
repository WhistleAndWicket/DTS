using Application.Features.TaskItem.Queries.Get;
using Domain.Exceptions;
using DTOs.Shared.Enums;
using Moq;
using Tests.Unit.Core;
using Tests.Unit.Helpers;

namespace Tests.Unit.Handlers.Queries
{
    /// <summary>
    /// Unit tests for <see cref="GetTaskItemQueryHandler"/>.
    /// Ensures that a task item retrieval behaves as expected.
    /// </summary>
    public class GetTaskItemQueryHandlerTest : HandlerTestBase
    {
        /// <summary>
        /// The handler for getting a task item.
        /// </summary>
        private readonly GetTaskItemQueryHandler _handler;

        /// <summary>
        /// Verifies that a task item is retrieved successfully.
        /// </summary>
        [Fact]
        public async Task Handle_ShouldReturnTaskItem_WhenTaskItemExists()
        {
            // Arrange
            var taskItem = TaskItemTestHelper.GetTaskItem();
            var query = new GetTaskItemQuery { Id = taskItem.Id };

            TaskItemRepositoryMock
                .Setup(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(taskItem);

            // Act.
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert.
            TaskItemRepositoryMock
                .Verify(r => r.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(taskItem.Id, result.Id);
            Assert.Equal(taskItem.Title, result.Title);
            Assert.Equal(taskItem.Description, result.Description);
            Assert.Equal(TaskItemStatus.Created, result.Status);
            Assert.Equal(taskItem.DueDate, result.DueDate);
        }

        /// <summary>
        /// Verifies that an exception is thrown when attempting to retrieve
        /// a TaskItem that does not exist.
        /// </summary>
        [Fact]
        public async Task Handle_ShouldThrowException_WhenTaskItemDoesNotExist()
        {
            // Arrange
            var command = new GetTaskItemQuery { Id = 999 };

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
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTaskItemQueryHandlerTest"/> class.
        /// </summary>
        public GetTaskItemQueryHandlerTest()
        {
            _handler = new(TaskItemRepositoryMock.Object);
        }
    }
}
