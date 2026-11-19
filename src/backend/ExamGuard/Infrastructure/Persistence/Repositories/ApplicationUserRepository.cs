using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.IRepositories;
using Domain.ValueObjects;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ApplicationUserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {                                      
            _dbContext = dbContext;
        }
        public async Task<List<Guid>> GetIdsOfAllProctorsAsync( CancellationToken cancellationToken = default)
        {
            return await (from userRole in _dbContext.UserRoles
                                join role in _dbContext.Roles on userRole.RoleId equals role.Id
                                where role.NormalizedName ==  UserRole.Proctor.ToString().ToUpper()
                                select userRole.UserId
                            ).ToListAsync(cancellationToken);            
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
