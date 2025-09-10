using Application.Common.RequestTypes;
using DTOs.Inputs.Commands.TaskItem;
using DTOs.Responses.TaskItem;

namespace Application.Features.TaskItem.Commands.Create
{
    /// <summary>
    /// Represents the command to create a TaskItem.
    /// </summary>
    public class CreateTaskItemCommand
        : CreateTaskItemCommandDto,
        ICommandWithReturnValue<TaskItemResponseDto>
    {
    }
}
