using Domain.Exceptions;
using Domain.Infrastructure.Repositories;
using MediatR;

namespace Application.Features.TaskItem.Commands.Delete
{
    /// <summary>
    /// Handles the command for deleting a TaskItem.
    /// </summary>
    /// <param name="taskItemRepository">The TaskItem repository.</param>
    public class DeleteTaskItemCommandHandler(ITaskItemRepository taskItemRepository)
        : IRequestHandler<DeleteTaskItemCommand>
    {
        /// <summary>
        /// The TaskItem repository.
        /// </summary>
        private readonly ITaskItemRepository _taskItemRepository = taskItemRepository;

        /// <inheritdoc/>
        /// <exception cref="EntityNotFoundException">When a task item cannot be found.</exception>
        public async Task Handle(DeleteTaskItemCommand request, CancellationToken cancellationToken)
        {
            // Check if the task item exist.
            var existingTaskItem =
                await _taskItemRepository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("The TaskItem cannot be found.");

            // Delete the Task Item.
            _taskItemRepository.Remove(existingTaskItem);
        }
    }
}
