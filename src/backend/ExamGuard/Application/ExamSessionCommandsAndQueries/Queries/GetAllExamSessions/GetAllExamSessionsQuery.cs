using Domain.Enums;
using Domain.Shared;
using MediatR;
namespace Application.ExamSessionCommandsAndQueries.Queries.GetAllExamSessions
{
    public sealed record GetAllExamSessionsQuery
        (int Page , int PageSize,string? Title , DateOnly? Date , string? SupervisorName ,string? ClassroomName ,OrderTypes? OrderType)
        :IRequest<PaginateList<GetAllExamSessionsResponse>>;
}
