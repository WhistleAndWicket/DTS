using Application.Common.RequestTypes;
using MediatR;

namespace Application.Common.Behaviours
{
    /// <summary>
    /// Implements a pipeline behavior for command handling in a <c>MediatR</c> application.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request, which must implement <see cref="ICommand"/>.</typeparam>
    /// <typeparam name="TResponse">The type of the response returned by the command handler.</typeparam>
    internal class CommandBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBehavior{TRequest, TResponse}"/> class.
        /// </summary>
        public CommandBehavior()
        {
        }

        /// <summary>
        /// Handles the request by invoking the next handler in the pipeline.
        /// </summary>
        /// <param name="request">The command request to be handled.</param>
        /// <param name="next">The delegate to invoke the next handler in the pipeline.</param>
        /// <param name="cancellationToken">A cancellation token for the operation.</param>
        /// <returns>The response of type <typeparamref name="TResponse"/>.</returns>
        public async Task<TResponse> Handle(
            TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            return await next(cancellationToken);
        }
    }
}
