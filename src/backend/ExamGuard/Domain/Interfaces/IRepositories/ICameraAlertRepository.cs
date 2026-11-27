using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces.IRepositories
{
    public interface ICameraAlertRepository : IRepository<CameraAlert>
    {
        Task<IReadOnlyList<CameraAlert>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<CameraAlert>> GetByExamSessionIdAsync(Guid examSessionId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<CameraAlert>> GetByCameraIdAsync(Guid cameraId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<CameraAlert>> GetByStatusAsync(AlertStatus status, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<CameraAlert>> GetPendingBySessionAsync(Guid examSessionId, CancellationToken cancellationToken = default);
    }
}
