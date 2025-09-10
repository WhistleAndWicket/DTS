
using MediatR;

namespace Application.Common.RequestTypes
{
    /// <summary>
    /// Represents a command with a return value.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response returned by the command.</typeparam>
    public interface ICommandWithReturnValue<TResponse> : IRequest<TResponse>, ICommand
    {
    }
}
