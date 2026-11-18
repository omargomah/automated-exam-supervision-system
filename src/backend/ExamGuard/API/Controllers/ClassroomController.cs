using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Features.CreateClassroom;
using Application.Features.AddSeatsToClassroom;

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
                ? StatusCode(StatusCodes.Status201Created, result)
                : BadRequest(result);
        }

        [HttpPost("{classroomId:guid}/seats")]
        [ProducesResponseType(typeof(Result<List<Guid>>), 201)]
        [ProducesResponseType(typeof(Result), 400)]
        [ProducesResponseType(typeof(Result), 404)]
        public async Task<IActionResult> AddSeatsToClassroom(
            [FromRoute] Guid classroomId,
            [FromBody] List<SeatDto> seats,
            CancellationToken cancellationToken)
        {
            var command = new AddSeatsToClassroomCommand(classroomId, seats);
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFail)
            {
                if (result.Error!.Code == "Classroom.NotFound")
                    return NotFound(result);

                return BadRequest(result);
            }

            return StatusCode(StatusCodes.Status201Created, result);
        }
    }
}
