using Domain.Entities;

namespace Domain.Interfaces.IRepositories
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}
