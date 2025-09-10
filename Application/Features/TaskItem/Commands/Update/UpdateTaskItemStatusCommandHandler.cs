using Domain.Enums;
using Domain.Exceptions;
using Domain.Infrastructure.Repositories;
using DTOs.Responses.TaskItem;
using Mapster;
using MediatR;

namespace Application.Features.TaskItem.Commands.Update
{
    /// <summary>
    /// Handles the command for updating the status of a TaskItem.
    /// </summary>
    /// <param name="taskItemRepository">The TaskItem repository.</param>
    public class UpdateTaskItemStatusCommandHandler(ITaskItemRepository taskItemRepository)
        : IRequestHandler<UpdateTaskItemStatusCommand, TaskItemResponseDto>
    {
        /// <summary>
        /// The TaskItem repository.
        /// </summary>
        private readonly ITaskItemRepository _taskItemRepository = taskItemRepository;

        /// <inheritdoc/>
        /// <exception cref="EntityNotFoundException">If the TaskItem does not exists.</exception>
        public async Task<TaskItemResponseDto> Handle(
            UpdateTaskItemStatusCommand command, CancellationToken cancellationToken)
        {
            // Get the task item.
            var taskItem = await _taskItemRepository.GetByIdAsync(command.Id, cancellationToken)
                ?? throw new EntityNotFoundException($"The TaskItem not found for the Id: {command.Id}");

            // Update the task item status.
            var newStatus = (TaskItemStatus)command.Status;
            taskItem.UpdateStatus(newStatus);

            // Map to its DTO.
            var result = taskItem.Adapt<TaskItemResponseDto>();
            return result;
        }
    }
}
