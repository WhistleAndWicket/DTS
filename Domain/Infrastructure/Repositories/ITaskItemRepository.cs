using Domain.Entities.Dts;

namespace Domain.Infrastructure.Repositories
{
    /// <summary>
    /// Defines the action required for the <see cref="TaskItem"/>.
    /// </summary>
    public interface ITaskItemRepository : IBaseRepository<TaskItem>
    {
        /// <summary>
        /// Removes the <see cref="TaskItem"/>.
        /// </summary>
        /// <param name="taskItem">The <see cref="TaskItem"/> to remove.</param>
        void Remove(TaskItem taskItem);
    }
}
