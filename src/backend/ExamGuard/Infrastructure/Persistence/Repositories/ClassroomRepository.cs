using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ClassroomRepository : Repository<Classroom>, IClassroomRepository
    {
        public ClassroomRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Classroom?> GetByNameAsync(string name, CancellationToken cancellationToken = default) =>
            await _set.SingleOrDefaultAsync(c => c.Name == name, cancellationToken);

        public async Task<Classroom?> GetWithSeatsAsync(Guid id, CancellationToken cancellationToken = default) =>
            await _set.Include(c => c.Seats).SingleOrDefaultAsync(c => c.Id == id, cancellationToken);

        public async Task<Classroom?> GetWithCamerasAsync(Guid id, CancellationToken cancellationToken = default) =>
            await _set.Include(c => c.CameraDevices).SingleOrDefaultAsync(c => c.Id == id, cancellationToken);

        public async Task<bool> IsNameUniqueAsync(string name, Guid? excludeClassroomId = null, CancellationToken cancellationToken = default) =>
            !await _set.AnyAsync(
                c => c.Name == name && (!excludeClassroomId.HasValue || c.Id != excludeClassroomId.Value),
                cancellationToken);
    }
}
