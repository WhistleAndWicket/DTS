using MediatR;

namespace Application.Common.RequestTypes
{
    /// <summary>
    /// Represents a query request with a response
    /// </summary>
    /// <typeparam name="TResponse">The response produced by the query.</typeparam>
    public interface IQuery<TResponse> : IRequest<TResponse>
    {
    }
}
