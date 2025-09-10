using Application.Common.RequestTypes;
using DTOs.Responses.TaskItem;

namespace Application.Features.TaskItem.Queries.Get
{
    /// <summary>
    /// The Query model for retrieving a TaskItem.
    /// </summary>
    public class GetTaskItemQuery : IQuery<TaskItemResponseDto>
    {
        /// <summary>
        /// The Id of the TaskItem.
        /// </summary>
        public int Id { get; set; }
    }
}
