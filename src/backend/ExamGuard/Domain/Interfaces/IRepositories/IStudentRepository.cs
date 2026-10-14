using Domain.Entities;

namespace Domain.Interfaces.IRepositories
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<IReadOnlyList<Student>> SearchByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Student>> GetStudentsByExamSessionAsync(Guid examSessionId, CancellationToken cancellationToken = default);
    }
}
