using API.Requests;
using Application.ExamSessionCommandsAndQueries.Commands.CreateExamSession;
using Asp.Versioning;
using Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ExamSessionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExamSessionController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [ProducesResponseType(typeof(Result), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = $"Supervisor")]
        public async Task<IActionResult> CreateExamSession([FromBody] CreateExamSessionCommandDto commandDto)
        {   // Get the supervisorId from the current user's claims and add it to the command before sending it to the mediator
            Guid supervisorId =  User.GetUserId();
            Result result = await _mediator.Send(commandDto.BuildAdapter()
                                           .AddParameters("SupervisorId", supervisorId)
                                           .AdaptToType<CreateExamSessionCommand>());
            if(result.IsSuccess)
                return Created();

            return BadRequest(result);
        }
    }
}
