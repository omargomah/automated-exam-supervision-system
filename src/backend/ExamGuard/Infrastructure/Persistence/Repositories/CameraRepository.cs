using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.IRepositories;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class CameraRepository : Repository<Camera>, ICameraRepository
    {
        public CameraRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<Camera>> GetByClassroomIdAsync(Guid classroomId, CancellationToken cancellationToken = default) =>
            await _set.Where(c => c.ClassroomId == classroomId).ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<Camera>> GetOnlineCamerasAsync(CancellationToken cancellationToken = default) =>
            await _set.Where(c => c.Status == DeviceStatus.Online).ToListAsync(cancellationToken);

        public async Task<Camera?> GetWithGridStartSeatAsync(Guid id, CancellationToken cancellationToken = default) =>
            await _set.Include(c => c.GridStartSeat).SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
}
