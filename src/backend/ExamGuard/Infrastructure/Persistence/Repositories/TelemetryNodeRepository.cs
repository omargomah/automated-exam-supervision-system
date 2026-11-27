using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.IRepositories;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class TelemetryNodeRepository : Repository<TelemetryNode>, ITelemetryNodeRepository
    {
        public TelemetryNodeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<TelemetryNode?> GetByMacAddressAsync(string macAddress, CancellationToken cancellationToken = default) =>
            await _set.SingleOrDefaultAsync(tn => tn.MacAddress == macAddress, cancellationToken);

        public async Task<IReadOnlyList<TelemetryNode>> GetByClassroomIdAsync(Guid classroomId, CancellationToken cancellationToken = default) =>
            await _set.Where(tn => tn.ClassroomId == classroomId).ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<TelemetryNode>> GetOnlineNodesAsync(CancellationToken cancellationToken = default) =>
            await _set.Where(tn => tn.Status == DeviceStatus.Online).ToListAsync(cancellationToken);

        public async Task<bool> IsMacAddressUniqueAsync(string macAddress, Guid? excludeNodeId = null, CancellationToken cancellationToken = default) =>
            !await _set.AnyAsync(
                tn => tn.MacAddress == macAddress && (!excludeNodeId.HasValue || tn.Id != excludeNodeId.Value),
                cancellationToken);
    }
}
