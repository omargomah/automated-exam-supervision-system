using Domain.Shared;
using Infrastructure.Options;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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

        [HttpPost("create")]
        [ProducesResponseType(typeof(Result), 201)]
        [ProducesResponseType(typeof(Result), 400)]
        public async Task<IActionResult> CreateClassroom([FromBody] CreateClassroomCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            else
            {
                return BadRequest(result.Error);
            }
        }
    }
}
