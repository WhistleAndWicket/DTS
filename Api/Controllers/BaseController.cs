using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// Serves as the base class for all API controllers in the application, providing common functionality and behaviors.
    /// </summary>
    /// <param name="mediator">The mediator for handling the actions.</param>
    /// <remarks>
    /// This base controller class applies the <see cref="ApiControllerAttribute"/> to ensure that derived controllers
    /// benefit from features such as automatic model validation, automatic HTTP 400 responses for invalid models,
    /// and other conventions provided by the <see cref="ApiControllerAttribute"/>.
    /// </remarks>
    [ApiController]
    public abstract class BaseController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// The mediator (MediatR) for handling the actions.
        /// </summary>
        protected readonly IMediator _mediator = mediator;
    }
}
