using Application.Features.GetStudentsWithPagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedStudentsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaginatedStudentsResponse>> GetStudents(
            [FromQuery] GetStudentsQuery query,
            CancellationToken cancellationToken) =>
            Ok(await _mediator.Send(query, cancellationToken));
    }
}
