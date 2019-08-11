using System;
using System.Linq;
using System.Threading.Tasks;

namespace JudgeSystem.Data.Common.Repositories
{
    public interface IRepository<TEntity> : IDisposable
        where TEntity : class
    {
        IQueryable<TEntity> All();

        IQueryable<TEntity> AllAsNoTracking();

        Task AddAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task<TEntity> FindAsync(object id);

        Task<int> SaveChangesAsync();
    }
}
