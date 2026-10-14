using Domain.Entities;

namespace Domain.Interfaces.IRepositories
{
    public interface IClassroomRepository : IRepository<Classroom>
    {
        Task<Classroom?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<Classroom?> GetWithSeatsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Classroom?> GetWithCamerasAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> IsNameUniqueAsync(string name, Guid? excludeClassroomId = null, CancellationToken cancellationToken = default);
    }
}
