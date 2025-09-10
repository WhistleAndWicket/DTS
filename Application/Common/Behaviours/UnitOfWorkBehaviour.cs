using Application.Common.RequestTypes;
using Domain.Infrastructure;
using MediatR;

namespace Application.Common.Behaviours
{
    /// <summary>
    /// Implements a pipeline behavior that manages unit of work transactions for command handling.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request, which must implement <see cref="IAsUnitOfWork"/>.</typeparam>
    /// <typeparam name="TResponse">The type of the response returned by the command handler.</typeparam>
    /// <param name="unitOfWork">The unit of work implementation to manage transactions.</param>
    public class UnitOfWorkBehaviour<TRequest, TResponse>(
        IUnitOfWork unitOfWork)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IAsUnitOfWork
    {
        /// <summary>
        /// The unit of work implementation to manage transactions.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        /// <summary>
        /// Handles the request by invoking the next handler in the pipeline
        /// and manages the unit of work transaction.
        /// </summary>
        /// <param name="request">The command request to be handled.</param>
        /// <param name="next">The delegate to invoke the next handler in the pipeline.</param>
        /// <param name="cancellationToken">A cancellation token for the operation.</param>
        /// <returns>The response of type <typeparamref name="TResponse"/>.</returns>
        public async Task<TResponse> Handle(
            TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next(cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return response;
        }
    }
}
