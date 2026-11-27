using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces.IRepositories
{
    public interface ITelemetryAlertRepository : IRepository<TelemetryAlert>
    {
        Task<IReadOnlyList<TelemetryAlert>> GetByExamSessionIdAsync(Guid examSessionId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<TelemetryAlert>> GetByTelemetryNodeIdAsync(Guid telemetryNodeId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<TelemetryAlert>> GetByStatusAsync(AlertStatus status, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<TelemetryAlert>> GetPendingBySessionAsync(Guid examSessionId, CancellationToken cancellationToken = default);
    }
}
