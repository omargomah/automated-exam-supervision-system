using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.IRepositories;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class TelemetryAlertRepository : Repository<TelemetryAlert>, ITelemetryAlertRepository
    {
        public TelemetryAlertRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<TelemetryAlert>> GetByExamSessionIdAsync(Guid examSessionId, CancellationToken cancellationToken = default) =>
            await _set.Where(ta => ta.ExamSessionId == examSessionId)
                      .OrderByDescending(ta => ta.CreatedAt)
                      .ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<TelemetryAlert>> GetByTelemetryNodeIdAsync(Guid telemetryNodeId, CancellationToken cancellationToken = default) =>
            await _set.Where(ta => ta.DeviceId == telemetryNodeId)
                      .OrderByDescending(ta => ta.CreatedAt)
                      .ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<TelemetryAlert>> GetByStatusAsync(AlertStatus status, CancellationToken cancellationToken = default) =>
            await _set.Where(ta => ta.Status == status)
                      .OrderByDescending(ta => ta.CreatedAt)
                      .ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<TelemetryAlert>> GetPendingBySessionAsync(Guid examSessionId, CancellationToken cancellationToken = default) =>
            await _set.Where(ta => ta.ExamSessionId == examSessionId && ta.Status == AlertStatus.Pending)
                      .OrderByDescending(ta => ta.CreatedAt)
                      .ToListAsync(cancellationToken);
    }
}
