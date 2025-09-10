using Application.Features.TaskItem.Commands.Create;
using Application.Features.TaskItem.Commands.Delete;
using Application.Features.TaskItem.Commands.Update;
using Application.Features.TaskItem.Queries.Get;
using Application.Features.TaskItem.Queries.GetAll;
using DTOs.Inputs.Commands.TaskItem;
using DTOs.Responses.TaskItem;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Api.Controllers
{
    /// <summary>
    /// The API for handling TaskItem actions.
    /// </summary>
    /// <param name="mediator">The mediator for handling the actions.</param>
    [Route("[controller]")]
    public class TaskItemController(IMediator mediator) : BaseController(mediator)
    {
        /// <summary>
        /// Creates a TaskItem.
        /// </summary>
        /// <param name="request">The request needed to create a TaskItem.</param>
        /// <response code="200">The response from creating the task item.</response>
        [HttpPost]
        public async Task<TaskItemResponseDto> Create([FromBody] CreateTaskItemCommandDto request)
        {
            var command = request.Adapt<CreateTaskItemCommand>();
            var response = await _mediator.Send(command);
            return response;
        }

        /// <summary>
        /// Deletes the TaskItem by its <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The Id of the TaskItem.</param>
        [HttpDelete]
        public async Task<IActionResult> Delete([Required] int id)
        {
            var command = new DeleteTaskItemCommand { Id = id };
            await _mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Get the TaskItem by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The Id of the TaskItem.</param>
        /// <response code="200">The TaskItem.</response>
        [HttpGet("{id}")]
        public async Task<TaskItemResponseDto> Get([Required] int id)
        {
            var query = new GetTaskItemQuery { Id = id };
            var result = await _mediator.Send(query);
            return result;
        }

        /// <summary>
        /// Gets all the TaskItem
        /// </summary>
        /// <returns>List of TaskItems.</returns>
        [HttpGet("get-all")]
        public async Task<List<TaskItemResponseDto>> GetAll()
        {
            var result = await _mediator.Send(new GetAllTaskItemsQuery());
            return result;
        }

        /// <summary>
        /// Updates the status of a TaskItem.
        /// </summary>
        /// <param name="id">The Id of the TaskItem.</param>
        /// <param name="request">The request needed to update the status of the TaskItem.</param>
        /// <response code="200">The Updated TaskItem.</response>
        [HttpPatch("{id}")]
        public async Task<TaskItemResponseDto> UpdateStatus(int id, [FromBody] UpdateTaskItemStatusCommandDto request)
        {
            var command = request.Adapt<UpdateTaskItemStatusCommand>();
            command.Id = id;
            var result = await _mediator.Send(command);
            return result;
        }
    }
}
