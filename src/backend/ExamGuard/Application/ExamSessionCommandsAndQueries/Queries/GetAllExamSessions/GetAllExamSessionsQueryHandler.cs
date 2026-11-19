using Domain.Enums;
using Domain.Interfaces.IRepositories;
using Domain.Shared;
using Mapster;
using MediatR;

namespace Application.ExamSessionCommandsAndQueries.Queries.GetAllExamSessions
{
    public class GetAllExamSessionsQueryHandler : IRequestHandler<GetAllExamSessionsQuery, PaginateList<GetAllExamSessionsResponse>>
    {
        private readonly IExamSessionRepository _examSessionRepository;

        public GetAllExamSessionsQueryHandler(IExamSessionRepository examSessionRepository)
        {
            _examSessionRepository = examSessionRepository;
        }
        public Task<PaginateList<GetAllExamSessionsResponse>> Handle(GetAllExamSessionsQuery request, CancellationToken cancellationToken)
        {
            // Start with the base query from the repository
            var query =  _examSessionRepository.GetQuery();

            // Apply filters and ordering based on the request parameters
            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                var searchTerm = request.Title.Trim();
                query = query.Where(es => es.Title.Contains(searchTerm));
            }

            if(request.Date.HasValue)
                query = query.Where(es => DateOnly.FromDateTime(es.StartTime) == request.Date.Value);

            if (!string.IsNullOrWhiteSpace(request.SupervisorName))
            {
                var searchTerm = request.SupervisorName.Trim();
                query = query.Where(es =>
                    (es.CreatedBy.FName + " " + es.CreatedBy.LName).Contains(searchTerm));
            }

            if(!string.IsNullOrWhiteSpace(request.ClassroomName))
            {
                var searchTerm = request.ClassroomName.Trim();
                query = query.Where(es => es.Classroom.Name.Contains(searchTerm));
            }

            if (request.OrderType.HasValue)
                query = request.OrderType.Value switch
                {
                    OrderTypes.Ascending => query.OrderBy(es => es.StartTime),
                    _ => query.OrderByDescending(es => es.StartTime),
                };
            else
                query = query.OrderByDescending(es => es.StartTime);

            // Project the filtered and ordered query to the pagination of GetAllExamSessionsResponse and paginate the results
            return _examSessionRepository.ExecutePaginationAsync(query.ProjectToType<GetAllExamSessionsResponse>(),request.Page,request.PageSize,cancellationToken);
        }
    }
}
