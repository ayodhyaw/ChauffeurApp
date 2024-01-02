using System.Linq.Expressions;

namespace ChauffeurApp.DataAccess.Repositories.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(long id);
        Task<IQueryable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Update(T entity);
        IQueryable<T> GetAllBySpec(Expression<Func<T, bool>> predicate, bool asNoTracking = true);
        Task<T?> GetBySpecAsync<Spec>(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<IQueryable<T>> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
        void SoftDelete(T entity);

    }
}
