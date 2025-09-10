using DTOs.CustomValidations;
using DTOs.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Inputs.Commands.TaskItem
{
    /// <summary>
    /// Represents the data transfer object for creating a TaskItem.
    /// </summary>
    public class CreateTaskItemCommandDto
    {
        /// <summary>
        /// The description of the task.
        /// </summary>        
        public string? Description { get; set; }

        /// <summary>
        /// The due date of the task.
        /// </summary>
        [Required]
        [ValidDate]
        public DateTime DueDate { get; set; }

        /// <summary>
        /// The status of the Task Item.
        /// </summary>
        [Required]
        [ValidEnum(typeof(TaskItemStatus))]
        public TaskItemStatus Status { get; set; } = TaskItemStatus.Created;

        /// <summary>
        /// The title of the task.
        /// </summary>
        [Required]
        public string Title { get; set; } = null!;
    }
}
