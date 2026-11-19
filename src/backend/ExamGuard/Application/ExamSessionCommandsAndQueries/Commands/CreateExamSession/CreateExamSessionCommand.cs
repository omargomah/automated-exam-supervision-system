using Domain.Shared;
using MediatR;

namespace Application.ExamSessionCommandsAndQueries.Commands.CreateExamSession
{
    public sealed record CreateExamSessionCommand(string Title, DateTime StartTime, DateTime EndTime , Guid ClassRoomId,Guid SupervisorId) : IRequest<Result>;
}
