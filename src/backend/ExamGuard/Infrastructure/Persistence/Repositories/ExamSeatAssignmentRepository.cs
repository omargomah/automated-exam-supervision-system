using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ExamSeatAssignmentRepository : Repository<ExamSeatAssignment>, IExamSeatAssignmentRepository
    {
        public ExamSeatAssignmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<ExamSeatAssignment>> GetByExamSessionIdAsync(Guid examSessionId, CancellationToken cancellationToken = default) =>
            await _set.Include(esa => esa.Student)
                      .Include(esa => esa.Seat)
                      .Where(esa => esa.ExamSessionId == examSessionId)
                      .ToListAsync(cancellationToken);

        public async Task<ExamSeatAssignment?> GetBySessionAndStudentAsync(Guid examSessionId, Guid studentId, CancellationToken cancellationToken = default) =>
            await _set.SingleOrDefaultAsync(
                esa => esa.ExamSessionId == examSessionId && esa.StudentId == studentId,
                cancellationToken);

        public async Task<ExamSeatAssignment?> GetBySessionAndSeatAsync(Guid examSessionId, Guid seatId, CancellationToken cancellationToken = default) =>
            await _set.SingleOrDefaultAsync(
                esa => esa.ExamSessionId == examSessionId && esa.SeatId == seatId,
                cancellationToken);

        public async Task<bool> IsSeatOccupiedAsync(Guid examSessionId, Guid seatId, Guid? excludeAssignmentId = null, CancellationToken cancellationToken = default) =>
            await _set.AnyAsync(
                esa => esa.ExamSessionId == examSessionId && esa.SeatId == seatId && (!excludeAssignmentId.HasValue || esa.Id != excludeAssignmentId.Value),
                cancellationToken);

        public async Task<bool> IsStudentAssignedAsync(Guid examSessionId, Guid studentId, Guid? excludeAssignmentId = null, CancellationToken cancellationToken = default) =>
            await _set.AnyAsync(
                esa => esa.ExamSessionId == examSessionId && esa.StudentId == studentId && (!excludeAssignmentId.HasValue || esa.Id != excludeAssignmentId.Value),
                cancellationToken);
    }
}
