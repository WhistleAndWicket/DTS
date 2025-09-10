using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Infrastructure.Middleware
{
    /// <summary>
    /// Middleware for handling global exceptions and returning standardized error responses
    /// using the <see cref="ProblemDetails"/> format (RFC 7807).
    /// </summary>
    /// <param name="next">
    /// The <see cref="RequestDelegate"/> representing the next middleware in the request pipeline.
    /// This delegate is invoked if no exception occurs in the current middleware.
    /// </param>
    /// <param name="logger">The <see cref="ILogger{TCategoryName}"/> used to log exceptions and related diagnostic information.</param>
    public class GlobalExceptionHandlerMiddleware(
        RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        /// <summary>
        /// The <see cref="RequestDelegate"/> representing the next middleware in the request pipeline.
        /// </summary>
        private readonly RequestDelegate _next = next;

        /// <summary>
        /// The <see cref="ILogger{TCategoryName}"/> used to log exceptions and related diagnostic information.
        /// </summary>
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger = logger;

        /// <summary>
        /// Mapping of exception types to corresponding HTTP status codes.
        /// </summary>
        private static readonly Dictionary<Type, int> _exceptionStatusCodeMapping = new()
        {
            { typeof(ArgumentException), StatusCodes.Status400BadRequest },
            { typeof(InvalidOperationException), StatusCodes.Status400BadRequest },
            { typeof(EntityNotFoundException), StatusCodes.Status404NotFound },
            { typeof(EntityAlreadyExistsException), StatusCodes.Status409Conflict },
        };

        /// <summary>
        /// Invokes the middleware, handling any exceptions thrown during the request pipeline execution.
        /// </summary>
        /// <param name="httpContext">The current <see cref="HttpContext"/>.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var statusCode = StatusCodes.Status500InternalServerError;

                if (_exceptionStatusCodeMapping.TryGetValue(ex.GetType(), out var mappedStatusCode))
                {
                    statusCode = mappedStatusCode;
                }

                // Log the exception
                _logger.LogError(ex, "Exception occurred. Status code: {StatusCode}", statusCode);

                await HandleExceptionAsync(httpContext, ex, statusCode);
            }
        }

        /// <summary>
        /// Handles exceptions by returning a <see cref="ProblemDetails"/> JSON response.
        /// </summary>
        /// <param name="context">The current <see cref="HttpContext"/>.</param>
        /// <param name="exception">The exception that was caught.</param>
        /// <param name="statusCode">The HTTP status code to return in the response.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = exception.GetType().Name,
                Detail = exception.Message,
            };

            var json = JsonConvert.SerializeObject(problemDetails);
            await context.Response.WriteAsync(json);
        }
    }
}
