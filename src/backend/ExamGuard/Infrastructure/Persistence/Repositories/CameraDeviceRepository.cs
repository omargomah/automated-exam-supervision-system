using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.IRepositories;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class CameraDeviceRepository : Repository<CameraDevice>, ICameraDeviceRepository
    {
        public CameraDeviceRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<CameraDevice?> GetByMacAddressAsync(string macAddress, CancellationToken cancellationToken = default) =>
            await _set.SingleOrDefaultAsync(c => c.MacAddress == macAddress, cancellationToken);

        public async Task<IReadOnlyList<CameraDevice>> GetByClassroomIdAsync(Guid classroomId, CancellationToken cancellationToken = default) =>
            await _set.Where(c => c.ClassroomId == classroomId).ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<CameraDevice>> GetOnlineCamerasAsync(CancellationToken cancellationToken = default) =>
            await _set.Where(c => c.Status == CameraStatus.Online).ToListAsync(cancellationToken);

        public async Task<bool> IsMacAddressUniqueAsync(string macAddress, Guid? excludeCameraId = null, CancellationToken cancellationToken = default) =>
            !await _set.AnyAsync(
                c => c.MacAddress == macAddress && (!excludeCameraId.HasValue || c.Id != excludeCameraId.Value),
                cancellationToken);
    }
}
