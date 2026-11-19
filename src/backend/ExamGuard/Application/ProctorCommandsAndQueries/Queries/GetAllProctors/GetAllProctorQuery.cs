using Domain.Shared;
using MediatR;
namespace Application.ProctorCommandsAndQueries.Queries.GetAllProctors
{
    public sealed record GetAllProctorQuery
        (int Page , int PageSize,string? Name)
        :IRequest<PaginateList<GetAllProctorResponse>>;
}
