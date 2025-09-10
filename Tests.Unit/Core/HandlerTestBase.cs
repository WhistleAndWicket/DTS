using Domain.Infrastructure;
using Domain.Infrastructure.Repositories;
using Moq;

namespace Tests.Unit.Core
{
    /// <summary>
    /// Base class for all handler unit tests.
    /// Provides common setup logic, including mocked dependencies
    /// and helper methods for creating handler instances.
    /// </summary>
    public abstract class HandlerTestBase
    {
        /// <summary>
        /// The mock repository for the TaskItem.
        /// </summary>
        protected readonly Mock<ITaskItemRepository> TaskItemRepositoryMock;

        /// <summary>
        /// The mock unit of work for handling persistence in tests.
        /// </summary>
        protected readonly Mock<IUnitOfWork> UnitOfWorkMock;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerTestBase"/> class,
        /// setting up default mocks for repository and unit of work.
        /// </summary>
        protected HandlerTestBase()
        {
            TaskItemRepositoryMock = new Mock<ITaskItemRepository>();
            UnitOfWorkMock = new Mock<IUnitOfWork>();
        }
    }
}
