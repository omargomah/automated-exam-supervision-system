using Domain.Enums;
namespace Application.ExamSessionCommandsAndQueries.Queries.GetAllExamSessions
{
    public sealed record GetAllExamSessionsResponse
            (Guid ExamSessionId,
            string ExamSessionTitle,
            DateTime StartTime,
            DateTime EndTime,
            ExamSessionStatus Status,
            string SupervisorName,
            string ClassroomName);
}
