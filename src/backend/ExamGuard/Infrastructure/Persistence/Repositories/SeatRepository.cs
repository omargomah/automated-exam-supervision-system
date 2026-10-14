using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class SeatRepository : Repository<Seat>, ISeatRepository
    {
        public SeatRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<Seat>> GetSeatsByClassroomIdAsync(Guid classroomId, CancellationToken cancellationToken = default) =>
            await _set.Where(s => s.ClassroomId == classroomId)
                      .OrderBy(s => s.Label)
                      .ToListAsync(cancellationToken);

        public async Task<Seat?> GetByClassroomAndLabelAsync(Guid classroomId, int label, CancellationToken cancellationToken = default) =>
            await _set.SingleOrDefaultAsync(s => s.ClassroomId == classroomId && s.Label == label, cancellationToken);

        public async Task<bool> IsLabelUniqueInClassroomAsync(Guid classroomId, int label, Guid? excludeSeatId = null, CancellationToken cancellationToken = default) =>
            !await _set.AnyAsync(
                s => s.ClassroomId == classroomId && s.Label == label && (!excludeSeatId.HasValue || s.Id != excludeSeatId.Value),
                cancellationToken);
    }
}
