using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using JudgeSystem.Common.Exceptions;
using JudgeSystem.Data.Common.Repositories;

using Microsoft.EntityFrameworkCore;

namespace JudgeSystem.Data.Repositories
{
    public class EfRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        public EfRepository(ApplicationDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            DbSet = Context.Set<TEntity>();
        }

        protected DbSet<TEntity> DbSet { get; set; }

        protected ApplicationDbContext Context { get; set; }

        public virtual IQueryable<TEntity> All() => DbSet;

        public virtual IQueryable<TEntity> AllAsNoTracking() => DbSet.AsNoTracking();

        public virtual async Task AddAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            await SaveChangesAsync();
        }

        public async Task<TEntity> FindAsync(object id)
        {
            TEntity model = await DbSet.FindAsync(id);

            if(model == null)
            {
                throw new EntityNotFoundException(typeof(TEntity).Name);
            }

            return model;
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
            await SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            DbSet.Remove(entity);
            await SaveChangesAsync();
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entites)
        {
            DbSet.RemoveRange(entites);
            await SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync() => Context.SaveChangesAsync();

        public void Dispose() => Context.Dispose();

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await Context.AddRangeAsync(entities);
            await Context.SaveChangesAsync();
        }
    }
}
