using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.IRepositories;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class DeviceRepository : Repository<Device>, IDeviceRepository
    {
        public DeviceRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<Device>> GetByClassroomIdAsync(Guid classroomId, CancellationToken cancellationToken = default) =>
            await _set.Where(d => d.ClassroomId == classroomId).ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<Device>> GetOnlineDevicesAsync(CancellationToken cancellationToken = default) =>
            await _set.Where(d => d.Status == DeviceStatus.Online).ToListAsync(cancellationToken);

        public async Task<Device?> GetByApiKeyHashAsync(string apiKeyHash, CancellationToken cancellationToken = default) =>
            await _set.SingleOrDefaultAsync(d => d.ApiKeyHash == apiKeyHash, cancellationToken);
    }
}
