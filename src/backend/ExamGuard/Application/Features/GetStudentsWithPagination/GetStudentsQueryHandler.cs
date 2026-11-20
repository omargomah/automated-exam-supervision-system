using Domain.Interfaces.IRepositories;
using MediatR;

namespace Application.Features.GetStudentsWithPagination
{
    public class GetStudentsQueryHandler : 
        IRequestHandler<GetStudentsQuery, PaginateList<PaginatedStudentsResponse>>
    {
        private readonly IStudentRepository _studentRepository;

        public GetStudentsQueryHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<PaginateList<PaginatedStudentsResponse>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
        {
            var query = _studentRepository.GetQuery();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();
                query = query.Where(s =>
                    (s.FName + " " + s.LName).Contains(search));
            }

            return await _studentRepository.ExecutePaginationAsync(
                query.OrderBy(s => s.FName).ThenBy(s => s.LName).ProjectToType<PaginatedStudentsResponse>(),
                request.Page,
                request.PageSize
                cancellationToken);

        }

    }
}
