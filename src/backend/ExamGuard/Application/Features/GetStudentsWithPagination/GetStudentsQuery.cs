using MediatR;

namespace Application.Features.GetStudentsWithPagination
{
    public sealed record PaginatedStudentsResponse(
        Guid Id,
        string FullName,
        string? StudentCode,
        string? Email
    );

    public record GetStudentsQuery(
        int PageNumber = 1,
        int PageSize = 10,
        string? Search = null
    ) : IRequest<PaginatedStudentsResponse>;
}
