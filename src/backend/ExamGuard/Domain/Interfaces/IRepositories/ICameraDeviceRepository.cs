using Domain.Entities;

namespace Domain.Interfaces.IRepositories
{
    public interface ICameraDeviceRepository : IRepository<CameraDevice>
    {
        Task<CameraDevice?> GetByMacAddressAsync(string macAddress, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<CameraDevice>> GetByClassroomIdAsync(Guid classroomId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<CameraDevice>> GetOnlineCamerasAsync(CancellationToken cancellationToken = default);
        Task<bool> IsMacAddressUniqueAsync(string macAddress, Guid? excludeCameraId = null, CancellationToken cancellationToken = default);
    }
}
