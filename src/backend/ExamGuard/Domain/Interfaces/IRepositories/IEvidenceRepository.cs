using Domain.Entities;

namespace Domain.Interfaces.IRepositories
{
    public interface IEvidenceRepository : IRepository<Evidence>
    {
        Task<IReadOnlyList<Evidence>> GetByAlertEventIdAsync(Guid alertEventId, CancellationToken cancellationToken = default);
    }
}
