using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces.IRepositories
{
    public interface IAlertEventRepository : IRepository<AlertEvent>
    {
        Task<IReadOnlyList<AlertEvent>> GetByExamSessionIdAsync(Guid examSessionId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<AlertEvent>> GetByStatusAsync(AlertStatus status, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<AlertEvent>> GetPendingAlertsBySessionAsync(Guid examSessionId, CancellationToken cancellationToken = default);
    }
}
