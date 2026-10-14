using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.IRepositories;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ExamSessionRepository : Repository<ExamSession>, IExamSessionRepository
    {
        public ExamSessionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ExamSession?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default) =>
            await _set.Include(es => es.Classroom)
                      .Include(es => es.CreatedBy)
                      .Include(es => es.ExamSeatAssignments)
                      .SingleOrDefaultAsync(es => es.Id == id, cancellationToken);

        public async Task<IReadOnlyList<ExamSession>> GetByClassroomIdAsync(Guid classroomId, CancellationToken cancellationToken = default) =>
            await _set.Where(es => es.ClassroomId == classroomId).ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<ExamSession>> GetByStatusAsync(ExamSessionStatus status, CancellationToken cancellationToken = default) =>
            await _set.Where(es => es.Status == status).ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<ExamSession>> GetUpcomingSessionsAsync(DateTime fromUtc, CancellationToken cancellationToken = default) =>
            await _set.Where(es => es.StartTime >= fromUtc && es.Status == ExamSessionStatus.Scheduled)
                      .OrderBy(es => es.StartTime)
                      .ToListAsync(cancellationToken);
    }
}
