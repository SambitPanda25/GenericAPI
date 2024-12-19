namespace GenericAPI.IRepository
{
    public interface IUnitofWork : IDisposable
    {
        IGenericRepository<T> GetRepository<T>() where T : class;
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollBackAsync();
    }
}
