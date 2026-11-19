using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Features.CreateClassroom;

namespace API.Controllers
{
    [Route("api/Classrooms")]
    [ApiController]
    public class ClassRommController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClassRommController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Result), 201)]
        [ProducesResponseType(typeof(Result), 400)]
        public async Task<IActionResult> CreateClassroom([FromBody] CreateClassroomCommand command, CancellationToken cancellationToken)
        {

            var result = await _mediator.Send(command, cancellationToken);
            return result.IsSuccess
                ? Created(result)
                : BadRequest(result);
        }
    }
}
