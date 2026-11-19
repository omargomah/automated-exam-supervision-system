using API.Requests;
using Application.Auth.CreateUserCommand;
using Application.ExamSessionCommandsAndQueries.Queries.GetAllExamSessions;
using Application.ProctorCommandsAndQueries.Queries.GetAllProctors;
using Asp.Versioning;
using Domain.Enums;
using Domain.Shared;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/v{version:apiVersion}/proctor")]
    [ApiController]
    [ApiVersion("1.0")]

    public class ProctorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProctorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// creates a new proctor account.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/v1.0/proctor/create-proctor
        ///     {
        ///        "email": "scdcdscsd@example.com",
        ///        "fname": "Omar",
        ///        "lname": "Gomaa",
        ///        "password": "Password123!",
        ///        "confirmPassword": "Password123!"
        ///     }
        ///
        /// </remarks>
        /// <param name="command">The proctor create payload containing proctor details.</param>
        /// <param name="cancellationToken">Cancellation token to observe while executing the request.</param>
        /// <returns>A <see cref="Result{T}"/> containing the created <see cref="CreateUserCommandResponse"/> details on success.</returns>
        /// <response code="200">
        /// proctor created successfully.
        /// 
        /// Sample response:
        /// <code>
        /// {
        ///   "isSuccess": true,
        ///   "value": {
        ///        "email": "organizer@example.com",
        ///        "fname": "Omar",
        ///        "lname": "Gomaa",
        ///        "userrole": "Proctor",
        ///   }
        /// }
        /// </code>
        /// </response>
        /// <response code="400">
        /// Registration failed due to validation errors (invalid userRole, invalid name, password mismatch, or duplicate email).
        /// 
        /// Sample response:
        /// <code>
        /// {
        ///   "isSuccess": false,
        ///   "error": {
        ///     "code": "Auth.InvalidCreationData",
        ///     "message": "email is invalid or already in use."
        ///   }
        /// }
        /// </code>
        /// </response>
        [HttpPost("create-proctor")]
        [ProducesResponseType(typeof(Result<CreateUserCommandResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Supervisor,ExamAdmin")]
        public async Task<ActionResult<Result<CreateUserCommandResponse>>> CreateUser([FromBody] CreateUserDto command, CancellationToken cancellationToken)
        {
            

            var result = await _mediator.Send(command.BuildAdapter().AddParameters("UserRole", UserRole.Proctor).AdaptToType<CreateUserCommand>(), cancellationToken);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet]
        [Authorize(Roles = "Supervisor,ExamAdmin")]
        [ProducesResponseType(typeof(PaginateList<GetAllProctorResponse>), StatusCodes.Status201Created)]
        public async Task<ActionResult<PaginateList<GetAllProctorResponse>>> GetAllProctors([FromQuery] GetAllProctorQuery query, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(query, cancellationToken));
        }



    }
}
