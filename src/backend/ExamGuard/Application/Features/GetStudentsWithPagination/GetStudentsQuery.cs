using Domain.Shared;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.GetStudentsWithPagination
{
    public sealed record PaginatedStudentsResponse(
        Guid Id,
        string FullName,
        string? StudentCode,
        string? Email,
        MediaFile ImageUrl
    );

    public record GetStudentsQuery(
        int PageNumber = 1,
        int PageSize = 10,
        string? Search = null
    ) : IRequest<PaginateList<PaginatedStudentsResponse>>;
}
