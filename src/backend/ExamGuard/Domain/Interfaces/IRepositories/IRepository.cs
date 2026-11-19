using Domain.Entities;
using Domain.Shared;

namespace Domain.Interfaces.IRepositories
{
    public interface IRepository<T> where T : Entity 
    {
        ValueTask AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<T?> GetAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> GetQuery();
        Task<PaginateList<itemType>> ExecutePaginationAsync<itemType>(IQueryable<itemType> query, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    }
}
