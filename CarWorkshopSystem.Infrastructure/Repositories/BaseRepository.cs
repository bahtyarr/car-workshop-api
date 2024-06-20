using CarWorkshopSystem.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarWorkshopSystem.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly CoreDbContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(CoreDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task BulkCreateAsync(List<T> entities)
        {
            await UseTransactionAsync(async context =>
            {
                await context.AddRangeAsync(entities);
                await _context.SaveChangesAsync();
            });
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        #region Protected Methods

        protected async Task<T?> ReadAsync(Func<DbSet<T>, IQueryable<T>> extendedQuery)
        {
            var entities = extendedQuery != null ? extendedQuery.Invoke(_dbSet) : _dbSet;
            return await entities.FirstOrDefaultAsync();
        }

        protected async Task<ICollection<T>> SearchAsync(Func<DbSet<T>, IQueryable<T>> extendedQuery)
        {
            var entities = extendedQuery != null ? extendedQuery.Invoke(_dbSet) : _dbSet;
            return await entities.ToListAsync();
        }

        protected async Task UseTransactionAsync(Func<CoreDbContext, Task> exec)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                await exec(_context);
                await transaction.CommitAsync();
            });
        }


        #endregion Protected Methods
    }
}