using Domain.Entities.Dts;
using Domain.Enums;
using Infrastructure.Persistence.Repositories;
using Tests.Unit.Helpers;

namespace Tests.Unit.Repositories
{
    /// <summary>
    /// Unit tests for <see cref="TaskItemRepository"/> class.
    /// </summary>
    public class TaskItemRepositoryTest : BaseRepositoryTestBase<TaskItem>
    {
        /// <summary>
        /// The TaskItem repository.
        /// </summary>
        private readonly TaskItemRepository _repository;

        /// <inheritdoc/>
        protected override TaskItem CreateEntity(string title = "")
        {
            return TaskItemTestHelper.GetTaskItem(title);
        }

        /// <summary>
        /// Verifies that a task item is removed successfully.
        /// </summary>
        [Fact]
        public async Task Remove_ShouldDeleteTaskItem()
        {
            // Arrange
            var task = new TaskItem { Title = "TaskToRemove", Status = TaskItemStatus.Created };
            await Context.Tasks.AddAsync(task);
            await Context.SaveChangesAsync();

            // Assert.
            Assert.NotNull(task);
            Assert.True(task.Id > 0);

            // Act
            _repository.Remove(task);
            await Context.SaveChangesAsync();

            // Assert
            var result = await _repository.GetByIdAsync(task.Id);
            Assert.Null(result);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskItemRepositoryTest"/> class.
        /// </summary>
        public TaskItemRepositoryTest() : base()
        {
            _repository = new(Context);
        }
    }
}
