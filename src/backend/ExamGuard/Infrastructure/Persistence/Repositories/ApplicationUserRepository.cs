using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Domain.ValueObjects;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var emailVoResult = Email.Create(email);
            if (emailVoResult.IsFail)
            {
                return null;
            }

            return await _set.SingleOrDefaultAsync(u => u.Email == emailVoResult.Value!, cancellationToken);
        }
    }
}
