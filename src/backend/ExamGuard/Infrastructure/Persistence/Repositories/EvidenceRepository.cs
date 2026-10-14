using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class EvidenceRepository : Repository<Evidence>, IEvidenceRepository
    {
        public EvidenceRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<Evidence>> GetByAlertEventIdAsync(Guid alertEventId, CancellationToken cancellationToken = default) =>
            await _set.Where(e => e.AlertEventId == alertEventId).ToListAsync(cancellationToken);
    }
}
