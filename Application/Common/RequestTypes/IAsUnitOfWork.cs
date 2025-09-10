using Application.Common.Behaviours;

namespace Application.Common.RequestTypes
{
    /// <summary>
    /// Represents a command that is associated with a unit of work.
    /// </summary>
    /// <remarks>
    /// This interface extends <see cref="ICommand"/> and is used to mark command requests 
    /// that should be processed within a unit of work transaction.<br/>
    /// Implementations of this interface are typically handled by a <see cref="UnitOfWorkBehaviour{TRequest, TResponse}"/>.
    /// </remarks>
    public interface IAsUnitOfWork : ICommand
    {
    }
}
