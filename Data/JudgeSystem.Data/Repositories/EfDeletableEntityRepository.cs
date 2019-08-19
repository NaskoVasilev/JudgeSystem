using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using JudgeSystem.Data.Common.Models;
using JudgeSystem.Data.Common.Repositories;

using Microsoft.EntityFrameworkCore;

namespace JudgeSystem.Data.Repositories
{
    public class EfDeletableEntityRepository<TEntity> : EfRepository<TEntity>, IDeletableEntityRepository<TEntity>
        where TEntity : class, IDeletableEntity
    {
        public EfDeletableEntityRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public override IQueryable<TEntity> All() => base.All().Where(x => !x.IsDeleted);

        public override IQueryable<TEntity> AllAsNoTracking() => base.AllAsNoTracking().Where(x => !x.IsDeleted);

        public IQueryable<TEntity> AllWithDeleted() => base.All().IgnoreQueryFilters();

        public IQueryable<TEntity> AllAsNoTrackingWithDeleted() => base.AllAsNoTracking().IgnoreQueryFilters();

        public Task<TEntity> GetByIdWithDeletedAsync(params object[] id)
        {
            Expression<Func<TEntity, bool>> byIdPredicate = EfExpressionHelper.BuildByIdPredicate<TEntity>(Context, id);

            return AllWithDeleted().FirstOrDefaultAsync(byIdPredicate);
        }

        public Task HardDeleteAsync(TEntity entity) => base.DeleteAsync(entity);

        public Task UndeleteAsync(TEntity entity)
        {
            entity.IsDeleted = false;
            entity.DeletedOn = null;

            return UpdateAsync(entity);
        }

        public override async Task DeleteAsync(TEntity entity)
        {
            SetEntityAsDeleted(entity);
            await UpdateAsync(entity);
        }

        public override async Task DeleteRangeAsync(IEnumerable<TEntity> entites)
        {
            foreach (TEntity entity in entites)
            {
                SetEntityAsDeleted(entity);
                DbSet.Update(entity);
            }

            await SaveChangesAsync();
        }

        private static void SetEntityAsDeleted(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
        }
    }
}
