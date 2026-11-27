using Domain.Entities;

namespace Domain.Interfaces.IRepositories
{
    public interface ITelemetryNodeRepository : IRepository<TelemetryNode>
    {
        Task<TelemetryNode?> GetByMacAddressAsync(string macAddress, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<TelemetryNode>> GetByClassroomIdAsync(Guid classroomId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<TelemetryNode>> GetOnlineNodesAsync(CancellationToken cancellationToken = default);
        Task<bool> IsMacAddressUniqueAsync(string macAddress, Guid? excludeNodeId = null, CancellationToken cancellationToken = default);
    }
}
