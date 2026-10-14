using Domain.Entities;

namespace Domain.Interfaces.IRepositories
{
    public interface IExamSeatAssignmentRepository : IRepository<ExamSeatAssignment>
    {
        Task<IReadOnlyList<ExamSeatAssignment>> GetByExamSessionIdAsync(Guid examSessionId, CancellationToken cancellationToken = default);
        Task<ExamSeatAssignment?> GetBySessionAndStudentAsync(Guid examSessionId, Guid studentId, CancellationToken cancellationToken = default);
        Task<ExamSeatAssignment?> GetBySessionAndSeatAsync(Guid examSessionId, Guid seatId, CancellationToken cancellationToken = default);
        Task<bool> IsSeatOccupiedAsync(Guid examSessionId, Guid seatId, Guid? excludeAssignmentId = null, CancellationToken cancellationToken = default);
        Task<bool> IsStudentAssignedAsync(Guid examSessionId, Guid studentId, Guid? excludeAssignmentId = null, CancellationToken cancellationToken = default);
    }
}
