using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Zust.Application.Repositories;
using Zust.Domain.Entities;
using Zust.Persistence.Contexts;

namespace Zust.Persistence.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : class,IBaseEntity,new()
    {

        private readonly AppDbContext _context;

        public ReadRepository(AppDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> method)
            =>await Table.AnyAsync(method);
        

        public async Task<int> CountAsync(Expression<Func<T, bool>>? method = null)
            =>method==null ? await Table.CountAsync() : await Table.CountAsync(method);

        public IQueryable<T> GetAll(Expression<Func<T, bool>>? method = null, params Expression<Func<T, object>>[] IncludeParams)
        {
            IQueryable<T> query = Table;
            if(method is not null)
                query = query.Where(method);
            if (IncludeParams.Any())
                query = IncludeParams.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> method, params Expression<Func<T, object>>[] IncludeParams)
        {
            IQueryable<T> query = Table;

            if (IncludeParams.Any())
                query = IncludeParams.Aggregate(query, (current, include) => current.Include(include));
            return await query.FirstOrDefaultAsync(method);
        }

        
    }
}
