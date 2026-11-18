using Domain.Shared;
using MediatR;

namespace Application.ExamSessionCommandsAndQueries.Commands.CreateExamSession
{
    public sealed record CreateExamSessionCommand(string title, DateTime startTime, DateTime endTime , Guid ClassRoomId,Guid SupervisorId) : IRequest<Result>;
}
