using DTOs.CustomValidations;
using DTOs.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Inputs.Commands.TaskItem
{
    /// <summary>
    /// Represents the data transfer object for updating the status of a TaskItem.
    /// </summary>
    public class UpdateTaskItemStatusCommandDto
    {
        /// <summary>
        /// The status of the TaskItem.
        /// </summary>
        [Required]
        [ValidEnum(typeof(TaskItemStatus))]
        public TaskItemStatus Status { get; set; }
    }
}
