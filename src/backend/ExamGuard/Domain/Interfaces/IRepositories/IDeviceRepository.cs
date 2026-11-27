using Domain.Entities;

namespace Domain.Interfaces.IRepositories
{
    public interface IDeviceRepository : IRepository<Device>
    {
        Task<IReadOnlyList<Device>> GetByClassroomIdAsync(Guid classroomId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Device>> GetOnlineDevicesAsync(CancellationToken cancellationToken = default);
        Task<Device?> GetByApiKeyHashAsync(string apiKeyHash, CancellationToken cancellationToken = default);
    }
}
