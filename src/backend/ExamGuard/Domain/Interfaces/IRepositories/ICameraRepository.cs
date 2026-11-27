using Domain.Entities;

namespace Domain.Interfaces.IRepositories
{
    public interface ICameraRepository : IRepository<Camera>
    {
        Task<IReadOnlyList<Camera>> GetByClassroomIdAsync(Guid classroomId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Camera>> GetOnlineCamerasAsync(CancellationToken cancellationToken = default);
        Task<Camera?> GetWithGridStartSeatAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
