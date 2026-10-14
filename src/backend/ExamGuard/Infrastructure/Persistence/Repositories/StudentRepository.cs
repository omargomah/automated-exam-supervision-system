using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<Student>> SearchByNameAsync(string name, CancellationToken cancellationToken = default) =>
            await _set.Where(s => s.FName.Contains(name) || s.LName.Contains(name))
                      .ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<Student>> GetStudentsByExamSessionAsync(Guid examSessionId, CancellationToken cancellationToken = default) =>
            await _set.Where(s => s.ExamSeatAssignments.Any(esa => esa.ExamSessionId == examSessionId))
                      .ToListAsync(cancellationToken);
    }
}
