using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.IRepositories;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class CameraAlertRepository : Repository<CameraAlert>, ICameraAlertRepository
    {
        public CameraAlertRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<CameraAlert>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default) =>
            await _set.Where(ca => ca.StudentId == studentId)
                      .OrderByDescending(ca => ca.CreatedAt)
                      .ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<CameraAlert>> GetByExamSessionIdAsync(Guid examSessionId, CancellationToken cancellationToken = default) =>
            await _set.Where(ca => ca.ExamSessionId == examSessionId)
                      .OrderByDescending(ca => ca.CreatedAt)
                      .ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<CameraAlert>> GetByCameraIdAsync(Guid cameraId, CancellationToken cancellationToken = default) =>
            await _set.Where(ca => ca.DeviceId == cameraId)
                      .OrderByDescending(ca => ca.CreatedAt)
                      .ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<CameraAlert>> GetByStatusAsync(AlertStatus status, CancellationToken cancellationToken = default) =>
            await _set.Where(ca => ca.Status == status)
                      .OrderByDescending(ca => ca.CreatedAt)
                      .ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<CameraAlert>> GetPendingBySessionAsync(Guid examSessionId, CancellationToken cancellationToken = default) =>
            await _set.Where(ca => ca.ExamSessionId == examSessionId && ca.Status == AlertStatus.Pending)
                      .OrderByDescending(ca => ca.CreatedAt)
                      .ToListAsync(cancellationToken);
    }
}
