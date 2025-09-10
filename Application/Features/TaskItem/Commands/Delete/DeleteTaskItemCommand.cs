using Application.Common.RequestTypes;

namespace Application.Features.TaskItem.Commands.Delete
{
    /// <summary>
    /// Represents the command to delete a TaskItem.
    /// </summary>
    public class DeleteTaskItemCommand : IVoidCommand, IAsUnitOfWork
    {
        /// <summary>
        /// The id of the TaskItem.
        /// </summary>
        public int Id { get; set; }
    }
}
