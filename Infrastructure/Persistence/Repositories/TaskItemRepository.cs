using Domain.Entities.Dts;
using Domain.Infrastructure.Repositories;

namespace Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Handles the action required for the <see cref="TaskItem"/>.
    /// </summary>
    /// <param name="dbContext">The <see cref="DtsDbContext"/>.</param>
    public class TaskItemRepository(DtsDbContext dbContext)
        : BaseRepository<TaskItem>(dbContext), ITaskItemRepository
    {
        /// <inheritdoc/>
        public void Remove(TaskItem taskItem)
        {
            dbContext.Tasks.Remove(taskItem);
        }
    }
}
