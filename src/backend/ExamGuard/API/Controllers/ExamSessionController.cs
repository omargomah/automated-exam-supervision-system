using API.Requests;
using Application.ExamSessionCommandsAndQueries.Commands.CreateExamSession;
using Application.ExamSessionCommandsAndQueries.Queries.GetAllExamSessions;
using Application.ProctorCommandsAndQueries.Queries.GetAllProctors;
using Asp.Versioning;
using Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/v{version:apiVersion}/exam-sessions")]
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
        [Authorize(Roles = "Supervisor,ExamAdmin")]
        public async Task<IActionResult> CreateExamSession([FromBody] CreateExamSessionCommandDto commandDto , CancellationToken cancellationToken)
        {   // Get the supervisorId from the current user's claims and add it to the command before sending it to the mediator
            Guid supervisorId =  User.GetUserId();
            Result result = await _mediator.Send(commandDto.BuildAdapter()
                                           .AddParameters("SupervisorId", supervisorId)
                                           .AdaptToType<CreateExamSessionCommand>(), cancellationToken);
            if(result.IsSuccess)
                return Created();

            return BadRequest(result);
        }

        [HttpGet]
        [Authorize(Roles = "Supervisor,ExamAdmin")]
        [ProducesResponseType(typeof(PaginateList<GetAllExamSessionsResponse>), StatusCodes.Status201Created)]
        public async Task<ActionResult<PaginateList<GetAllExamSessionsResponse>>> GetAllExamSessions([FromQuery]GetAllExamSessionsQuery query, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(query, cancellationToken));                        
        }
    }
}
