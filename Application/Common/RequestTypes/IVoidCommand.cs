using MediatR;

namespace Application.Common.RequestTypes
{
    /// <summary>
    /// Represents a command that does not return a result.
    /// </summary>
    /// <remarks>
    /// This interface combines the functionality of the MediatR <see cref="IRequest"/> and the <see cref="ICommand"/>.<br/>
    /// Implementations of this interface are used for commands that perform an action without returning data.
    /// </remarks>
    public interface IVoidCommand : IRequest, ICommand
    {
    }
}
