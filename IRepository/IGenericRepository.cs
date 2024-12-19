using System.Linq.Expressions;

namespace GenericAPI.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIDAsync(int Id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task DeleteAsync(T entity);
        Task UpdateAsync(T entity);
    }
}
