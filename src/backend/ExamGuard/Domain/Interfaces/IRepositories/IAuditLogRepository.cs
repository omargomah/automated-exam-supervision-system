using Domain.Entities;

namespace Domain.Interfaces.IRepositories
{
    public interface IAuditLogRepository : IRepository<AuditLog>
    {
        Task<IReadOnlyList<AuditLog>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<AuditLog>> GetByEntityAsync(string entityName, Guid? entityId = null, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<AuditLog>> GetRecentLogsAsync(int count, CancellationToken cancellationToken = default);
    }
}
