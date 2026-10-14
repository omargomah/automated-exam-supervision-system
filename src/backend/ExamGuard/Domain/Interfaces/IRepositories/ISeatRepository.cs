using Domain.Entities;

namespace Domain.Interfaces.IRepositories
{
    public interface ISeatRepository : IRepository<Seat>
    {
        Task<IReadOnlyList<Seat>> GetSeatsByClassroomIdAsync(Guid classroomId, CancellationToken cancellationToken = default);
        Task<Seat?> GetByClassroomAndLabelAsync(Guid classroomId, int label, CancellationToken cancellationToken = default);
        Task<bool> IsLabelUniqueInClassroomAsync(Guid classroomId, int label, Guid? excludeSeatId = null, CancellationToken cancellationToken = default);
    }
}
