using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PostService.DAL
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        protected readonly PostDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(PostDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        public async virtual Task<IEnumerable<TEntity>> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includedProperties = ""
        )
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includedProperty in includedProperties.Split(",", StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includedProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        public async virtual Task<TEntity> GetById(object id)
        {
            return await _dbSet.FindAsync(id);
        }
        
        public async Task<bool> Exists(int id)
        {
            return await GetById(id) != null;
        }

        public async virtual Task Add(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(object id)
        {
            Delete(GetById(id));
        }

        public virtual void Delete(TEntity entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _dbSet.Remove(entity);
        }
    }
}
