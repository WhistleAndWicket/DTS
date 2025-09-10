using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities.Dts
{
    /// <summary>
    /// The DB class for the Task.
    /// </summary>
    public class TaskItem : Entity
    {
        /// <summary>
        /// The description of the task.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The due date of the task.
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// The status of the task.
        /// </summary>
        public TaskItemStatus Status { get; set; } = TaskItemStatus.Created;

        /// <summary>
        /// The title of the task.
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// Updates the status of a Task.
        /// </summary>
        /// <param name="newStatus">The status to update.</param>
        /// <exception cref="ArgumentException">If the <paramref name="newStatus"/> is empty or whitespace.</exception>
        public void UpdateStatus(TaskItemStatus newStatus)
        {
            if (newStatus != Status)
            {
                Status = newStatus;
            }
        }
    }
}
