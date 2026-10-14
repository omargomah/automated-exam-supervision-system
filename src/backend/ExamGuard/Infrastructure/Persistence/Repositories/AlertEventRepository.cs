using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.IRepositories;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class AlertEventRepository : Repository<AlertEvent>, IAlertEventRepository
    {
        public AlertEventRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<AlertEvent?> GetWithEvidencesAsync(Guid id, CancellationToken cancellationToken = default) =>
            await _set.Include(ae => ae.Evidences)
                      .Include(ae => ae.CameraDevice)
                      .Include(ae => ae.Student)
                      .SingleOrDefaultAsync(ae => ae.Id == id, cancellationToken);

        public async Task<IReadOnlyList<AlertEvent>> GetByExamSessionIdAsync(Guid examSessionId, CancellationToken cancellationToken = default) =>
            await _set.Where(ae => ae.ExamSessionId == examSessionId)
                      .OrderByDescending(ae => ae.CreatedAt)
                      .ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<AlertEvent>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default) =>
            await _set.Where(ae => ae.StudentId == studentId)
                      .OrderByDescending(ae => ae.CreatedAt)
                      .ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<AlertEvent>> GetByStatusAsync(AlertStatus status, CancellationToken cancellationToken = default) =>
            await _set.Where(ae => ae.Status == status)
                      .OrderByDescending(ae => ae.CreatedAt)
                      .ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<AlertEvent>> GetPendingAlertsBySessionAsync(Guid examSessionId, CancellationToken cancellationToken = default) =>
            await _set.Where(ae => ae.ExamSessionId == examSessionId && ae.Status == AlertStatus.Pending)
                      .OrderByDescending(ae => ae.CreatedAt)
                      .ToListAsync(cancellationToken);
    }
}
