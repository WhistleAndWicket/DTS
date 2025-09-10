using Domain.Exceptions;
using Domain.Infrastructure.Repositories;
using DTOs.Responses.TaskItem;
using Mapster;
using MediatR;

namespace Application.Features.TaskItem.Queries.Get
{
    /// <summary>
    /// Handles the query handler for retrieving a TaskItem.
    /// </summary>
    /// <param name="taskItemRepository">The TaskItem repository.</param>
    public class GetTaskItemQueryHandler(ITaskItemRepository taskItemRepository)
        : IRequestHandler<GetTaskItemQuery, TaskItemResponseDto>
    {
        /// <summary>
        /// The TaskItem repository.
        /// </summary>
        private readonly ITaskItemRepository _taskItemRepository = taskItemRepository;

        /// <inheritdoc/>
        /// <exception cref="EntityNotFoundException">If the TaskItem does not exists.</exception>
        public async Task<TaskItemResponseDto> Handle(GetTaskItemQuery query, CancellationToken cancellationToken)
        {
            // Get the task item.
            var taskItem = await _taskItemRepository.GetByIdAsync(query.Id, cancellationToken)
                ?? throw new EntityNotFoundException($"The Task Item not found for the Id: {query.Id}");

            // Map the task item to its DTO.
            var result = taskItem.Adapt<TaskItemResponseDto>();
            return result;
        }
    }
}
