using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Data.Common.Models;

namespace JudgeSystem.Data.Common.Repositories
{
    public interface IDeletableEntityRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IDeletableEntity
    {
        IQueryable<TEntity> AllWithDeleted();

        IQueryable<TEntity> AllAsNoTrackingWithDeleted();

        Task<TEntity> GetByIdWithDeletedAsync(params object[] id);

        Task HardDeleteAsync(TEntity entity);

        Task UndeleteAsync(TEntity entity);
    }
}
