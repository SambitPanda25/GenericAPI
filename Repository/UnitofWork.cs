using GenericAPI.Context;
using GenericAPI.IRepository;
using Microsoft.EntityFrameworkCore.Storage;

namespace GenericAPI.Repository
{
    public class UnitofWork : IUnitofWork
    {
        private readonly CommonDbContext _dbContext;

        private IDbContextTransaction _transaction;
        private readonly Dictionary<Type, object> _repositories;

        public UnitofWork(CommonDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Dictionary<Type, object>();
        }
        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await _transaction.CommitAsync();
            }
            catch
            {
                await _transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null!;
            }
        }

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return _repositories[typeof(T)] as IGenericRepository<T>;
            }

            var repository = new GenericRepository<T>(_dbContext);
            _repositories.Add(typeof(T), repository);
            return repository;
        }

        public async Task RollBackAsync()
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction= null!;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync(); 
        }
    }
}
