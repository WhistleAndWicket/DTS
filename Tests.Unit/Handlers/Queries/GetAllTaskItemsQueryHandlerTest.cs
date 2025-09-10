using Application.Common.Specifications;
using Application.Features.TaskItem.Queries.GetAll;
using Domain.Entities.Dts;
using DTOs.Shared.Enums;
using Moq;
using Tests.Unit.Core;
using Tests.Unit.Helpers;

namespace Tests.Unit.Handlers.Queries
{
    /// <summary>
    /// Unit tests for <see cref="GetAllTaskItemsQueryHandler"/>.
    /// Ensures that the task items retrieval behaves as expected.
    /// </summary>
    public class GetAllTaskItemsQueryHandlerTest : HandlerTestBase
    {
        /// <summary>
        /// The handler for retrieving all the task items.
        /// </summary>
        private readonly GetAllTaskItemsQueryHandler _handler;

        /// <summary>
        /// Verifies that all the task items are retrieved successfully.
        /// </summary>
        /// <param name="setupTaskItem">Indicates whether to setup a task item.</param>
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Handle_ShouldReturnAllTaskItems_WhenTaskItemsExists(bool setupTaskItem)
        {
            // Arrange
            var taskItem = TaskItemTestHelper.GetTaskItem();
            var query = new GetAllTaskItemsQuery();

            TaskItemRepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<DefaultSpecification<TaskItem>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(setupTaskItem ? [taskItem] : []);

            // Act.
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert.
            TaskItemRepositoryMock
                .Verify(r => r.GetAllAsync(
                    It.IsAny<DefaultSpecification<TaskItem>>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
            Assert.NotNull(result);
            if (setupTaskItem)
            {
                Assert.NotEmpty(result);
                Assert.Single(result);
                Assert.Equal(taskItem.Id, result[0].Id);
                Assert.Equal(taskItem.Title, result[0].Title);
                Assert.Equal(taskItem.Description, result[0].Description);
                Assert.Equal(TaskItemStatus.Created, result[0].Status);
                Assert.Equal(taskItem.DueDate, result[0].DueDate);
            }
            else
            {
                Assert.Empty(result);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllTaskItemsQueryHandlerTest"/> class.
        /// </summary>
        public GetAllTaskItemsQueryHandlerTest()
        {
            _handler = new(TaskItemRepositoryMock.Object);
        }
    }
}
