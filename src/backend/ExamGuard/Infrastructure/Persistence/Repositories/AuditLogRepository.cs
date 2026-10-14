using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<AuditLog>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default) =>
            await _set.Where(al => al.ApplicationUserId == userId)
                      .OrderByDescending(al => al.CreatedAt)
                      .ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<AuditLog>> GetByEntityAsync(string entityName, Guid? entityId = null, CancellationToken cancellationToken = default) =>
            await _set.Where(al => al.EntityName == entityName && (!entityId.HasValue || al.EntityId == entityId.Value))
                      .OrderByDescending(al => al.CreatedAt)
                      .ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<AuditLog>> GetRecentLogsAsync(int count, CancellationToken cancellationToken = default) =>
            await _set.OrderByDescending(al => al.CreatedAt)
                      .Take(count)
                      .ToListAsync(cancellationToken);
    }
}
