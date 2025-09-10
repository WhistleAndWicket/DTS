using Application.Common.RequestTypes;
using DTOs.Inputs.Commands.TaskItem;
using DTOs.Responses.TaskItem;

namespace Application.Features.TaskItem.Commands.Update
{
    /// <summary>
    /// Represents the command to update the Status of a TaskItem.
    /// </summary>
    public class UpdateTaskItemStatusCommand 
        : UpdateTaskItemStatusCommandDto, 
        ICommandWithReturnValue<TaskItemResponseDto>, 
        IAsUnitOfWork
    {
        /// <summary>
        /// The id of the TaskItem.
        /// </summary>
        public int Id { get; set; }
    }
}
