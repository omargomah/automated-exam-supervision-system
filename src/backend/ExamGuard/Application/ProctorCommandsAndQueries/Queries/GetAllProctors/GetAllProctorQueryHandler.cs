using Application.ExamSessionCommandsAndQueries.Queries.GetAllExamSessions;
using Domain.Enums;
using Domain.Interfaces.IRepositories;
using Domain.Shared;
using Mapster;
using MediatR;

namespace Application.ProctorCommandsAndQueries.Queries.GetAllProctors
{
    public class GetAllProctorQueryHandler : IRequestHandler<GetAllProctorQuery, PaginateList<GetAllProctorResponse>>
    {
        private readonly IApplicationUserRepository _applicationUserRepository;

        public GetAllProctorQueryHandler(IApplicationUserRepository applicationUserRepository)
        {
            _applicationUserRepository = applicationUserRepository;
        }
        public async Task<PaginateList<GetAllProctorResponse>> Handle(GetAllProctorQuery request, CancellationToken cancellationToken)
        {
            // Retrieve the IDs of all proctors from the repository
            var proctorIds = await _applicationUserRepository.GetIdsOfAllProctorsAsync(cancellationToken);
            
            
            // Start with the base query from the repository
            var query =  _applicationUserRepository.GetQuery().Where(u => proctorIds.Contains(u.Id));

            // Apply filters and ordering based on the request parameters

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var searchTerm = request.Name.Trim();
                query = query.Where(es =>
                    (es.FName + " " + es.LName).Contains(searchTerm));
            }

            // Project the filtered and ordered query to the pagination of GetAllExamSessionsResponse and paginate the results
            return  await _applicationUserRepository.ExecutePaginationAsync(query.ProjectToType<GetAllProctorResponse>(),request.Page,request.PageSize,cancellationToken);
        }
    }
}
