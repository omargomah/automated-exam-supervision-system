using MediatR;

namespace Application.Features.GetStudentsWithPagination
{
    public sealed record StudentItemDto(
        Guid Id,
        string FullName,
        string? StudentCode,
        string? Email
    );

    public sealed record PaginatedStudentsResponse(
        IReadOnlyList<StudentItemDto> Items,
        int PageNumber,
        int PageSize,
        int TotalCount,
        int TotalPages
    );

    public record GetStudentsQuery(
        int PageNumber = 1,
        int PageSize = 10,
        string? Search = null
    ) : IRequest<PaginatedStudentsResponse>;

    //// For backwards compatibility and Controller binding
    //public sealed record GetStudentWithPaginationCommand(
    //    int PageNumber = 1,
    //    int PageSize = 10,
    //    string? Search = null
    //) : GetStudentsQuery(PageNumber, PageSize, Search);

    //public sealed record GetStudentQuery(
    //    int PageNumber = 1,
    //    int PageSize = 10,
    //    string? Search = null
    //) : GetStudentsQuery(PageNumber, PageSize, Search);
}
