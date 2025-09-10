using Domain.Infrastructure;
using Domain.Infrastructure.Repositories;
using DTOs.Responses.TaskItem;
using Mapster;
using MediatR;

namespace Application.Features.TaskItem.Commands.Create
{
    /// <summary>
    /// Handles the command for creating a TaskItem.
    /// </summary>
    /// <param name="taskItemRepository">The TaskItem repository.</param>
    /// <param name="unitOfWork">The transaction support.</param>
    public class CreateTaskItemCommandHandler(
        ITaskItemRepository taskItemRepository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<CreateTaskItemCommand, TaskItemResponseDto>
    {
        /// <summary>
        /// The TaskItem repository.
        /// </summary>
        private readonly ITaskItemRepository _taskItemRepository = taskItemRepository;

        /// <summary>
        /// The transaction support.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        /// <inheritdoc/>
        public async Task<TaskItemResponseDto> Handle(CreateTaskItemCommand command, CancellationToken cancellationToken)
        {
            // Construct the task item.
            Domain.Entities.Dts.TaskItem taskItemToCreate = new()
            {
                Description = command.Description,
                DueDate = command.DueDate,
                Status = Domain.Enums.TaskItemStatus.Created,
                Title = command.Title,
            };

            // Create the task item.
            var createdTaskItem =
                await _taskItemRepository.AddAsync(taskItemToCreate, cancellationToken);

            // Save the changes and commit the transaction.
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Map to its DTO.
            var result = createdTaskItem.Adapt<TaskItemResponseDto>();
            return result;
        }
    }
}
