using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Domain.Shared;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly ApplicationDbContext _dbContext;
        protected readonly DbSet<T> _set;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _set = dbContext.Set<T>();
        }
        public IQueryable<T> GetQuery() => _set.AsQueryable();
        public async Task<PaginateList<itemType>> ExecutePaginationAsync<itemType>(IQueryable<itemType> query, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            if (page < 1)
                page = 1;

            if (pageSize < 1)
                pageSize = 10;

            List<itemType> items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return PaginateList<itemType>.CreateAsync(items, await query.CountAsync(cancellationToken), page, pageSize);
        }

        public async ValueTask AddAsync(T entity, CancellationToken cancellationToken = default) =>
            await _set.AddAsync(entity, cancellationToken);

        public Task<T?> GetAsync(Guid id, CancellationToken cancellationToken = default) =>
            _set.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default) =>
            await _set.ToListAsync(cancellationToken);

        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default) =>
            _set.AnyAsync(x => x.Id == id, cancellationToken);

        public void Update(T entity) => _set.Update(entity);

        public void Delete(T entity) => _set.Remove(entity);
    }
}
