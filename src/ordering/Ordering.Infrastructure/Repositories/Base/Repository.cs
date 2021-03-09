using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities.Base;
using Ordering.Core.Repositories.Base;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected readonly OrderDbContext Context;

        public Repository(OrderDbContext context)
        {
            Context = context;
        }

        public async Task<IReadOnlyList<T>> GetAsync() => await Context.Set<T>().ToListAsync();

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate) =>
            await Context.Set<T>().Where(predicate).ToListAsync();

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate,
                                                     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                     string includeString = null, bool disableTracking = true)
        {
            var q = Context.Set<T>().AsQueryable();

            if (disableTracking) q = q.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(includeString)) q = q.Include(includeString);

            return predicate is null || orderBy is null ? await q.ToListAsync() : await orderBy(q).ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                                     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                     List<Expression<Func<T, object>>> includes = null,
                                                     bool disableTracking = true)
        {
            var q = Context.Set<T>().AsQueryable();

            if (disableTracking) q = q.AsNoTracking();

            if (includes != null) q = includes.Aggregate(q, (current, include) => current.Include(include));

            return predicate is null || orderBy is null ? await q.ToListAsync() : await orderBy(q).ToListAsync();
        }

        public async Task<T> GetAsync(object id) => await Context.Set<T>().FindAsync(id);

        public async Task<T> CreateAsync(T obj)
        {
            await Context.Set<T>().AddAsync(obj);
            await Context.SaveChangesAsync();
            return obj;
        }

        public async Task<T> UpdateAsync(T obj)
        {
            Context.Entry(obj).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return obj;
        }

        public async Task<bool> DeleteAsync(T obj)
        {
            Context.Set<T>().Remove(obj);
            await Context.SaveChangesAsync();
            return true;
        }
    }
}