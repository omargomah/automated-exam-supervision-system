using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces.IRepositories
{
    public interface IExamSessionRepository : IRepository<ExamSession>
    {
        Task<ExamSession?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ExamSession>> GetByClassroomIdAsync(Guid classroomId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ExamSession>> GetByStatusAsync(ExamSessionStatus status, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ExamSession>> GetUpcomingSessionsAsync(DateTime fromUtc, CancellationToken cancellationToken = default);
    }
}
