using Domain.Interfaces.IRepositories;
using MediatR;

namespace Application.Features.GetStudentsWithPagination
{
    public class GetStudentsQueryHandler : 
        IRequestHandler<GetStudentsQuery, PaginatedStudentsResponse>
    {
        private readonly IStudentRepository _studentRepository;

        public GetStudentsQueryHandler(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<PaginatedStudentsResponse> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
        {
            var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
            var pageSize = request.PageSize < 1 ? 10 : (request.PageSize > 50 ? 50 : request.PageSize);

            var query = _studentRepository.GetQuery();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();
                query = query.Where(s =>
                    s.FName.Contains(search) ||
                    s.LName.Contains(search) ||
                    (s.FName + " " + s.LName).Contains(search));
            }

            var paginatedResult = await _studentRepository.ExecutePaginationAsync(
                query.OrderBy(s => s.FName).ThenBy(s => s.LName),
                pageNumber,
                pageSize,
                cancellationToken);

            var items = paginatedResult.Items
                .Select(s => new StudentItemDto(
                    s.Id,
                    s.FullName,
                    null,
                    null))
                .ToList();

            var totalPages = paginatedResult.TotalCount == 0
                ? 0
                : (int)Math.Ceiling(paginatedResult.TotalCount / (double)paginatedResult.PageSize);

            return new PaginatedStudentsResponse(
                items,
                paginatedResult.Page,
                paginatedResult.PageSize,
                paginatedResult.TotalCount,
                totalPages
            );
        }

    }
}
