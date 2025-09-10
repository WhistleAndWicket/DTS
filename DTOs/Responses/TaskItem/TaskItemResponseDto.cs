using DTOs.Shared.Enums;

namespace DTOs.Responses.TaskItem
{
    /// <summary>
    /// The DTO for the TaskItem.
    /// </summary>
    public class TaskItemResponseDto
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
        /// The Id of the Task.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The status of the task.
        /// </summary>
        public TaskItemStatus Status { get; set; }

        /// <summary>
        /// The title of the task.
        /// </summary>
        public string Title { get; set; } = null!;
    }
}
