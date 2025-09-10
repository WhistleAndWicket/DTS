using Application.Common.RequestTypes;
using DTOs.Responses.TaskItem;

namespace Application.Features.TaskItem.Queries.GetAll
{
    /// <summary>
    /// The Query model for retrieving all TaskItems.
    /// </summary>
    public class GetAllTaskItemsQuery : IQuery<List<TaskItemResponseDto>>
    {
    }
}
