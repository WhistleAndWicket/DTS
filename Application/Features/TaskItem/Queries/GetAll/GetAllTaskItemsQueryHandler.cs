using Application.Common.Specifications;
using Domain.Infrastructure.Repositories;
using DTOs.Responses.TaskItem;
using Mapster;
using MediatR;

namespace Application.Features.TaskItem.Queries.GetAll
{
    /// <summary>
    /// Handles the query handler for retrieving all TaskItems.
    /// </summary>
    /// <param name="taskItemRepository">The TaskItem repository.</param>
    public class GetAllTaskItemsQueryHandler(ITaskItemRepository taskItemRepository)
        : IRequestHandler<GetAllTaskItemsQuery, List<TaskItemResponseDto>>
    {
        /// <summary>
        /// The TaskItem repository.
        /// </summary>
        private readonly ITaskItemRepository _taskItemRepository = taskItemRepository;

        /// <inheritdoc/>
        public async Task<List<TaskItemResponseDto>> Handle(
            GetAllTaskItemsQuery request, CancellationToken cancellationToken)
        {
            // Setup the specification needed for the task item.
            var specification = new DefaultSpecification<Domain.Entities.Dts.TaskItem>();

            // Get all the task items with the total count for the page.
            var allTaskItems =
                await _taskItemRepository.GetAllAsync(specification, cancellationToken);
            var result = allTaskItems.Adapt<List<TaskItemResponseDto>>();
            return result;
        }
    }
}
