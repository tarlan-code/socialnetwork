using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using Zust.Application.Repositories;
using Zust.Domain.Entities;
using Zust.Persistence.Contexts;

namespace Zust.Persistence.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : class, IBaseEntity,new()
    {
        readonly AppDbContext _context;

        public WriteRepository(AppDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public async Task<bool> AddAsync(T model)
        {
            EntityEntry<T> entityEntry =  await Table.AddAsync(model);
            return entityEntry.State == EntityState.Added;
        }

        public async Task<bool> AddRangeAsync(List<T> models)
        {
            await Table.AddRangeAsync(models);
            return true;
        }

        public async Task<bool> Delete(Expression<Func<T,bool>> method)
        {
            T model = await Table.FirstOrDefaultAsync(method);
            return Delete(model);
        }

        public bool Delete(T model)
        {
            if(model is not null)
            {
                EntityEntry<T> entityEntry = Table.Remove(model);
                return entityEntry.State == EntityState.Deleted;
            }
            return false;
        }

        public bool DeleteRange(List<T> models)
        {
            Table.RemoveRange(models);
            return true;
        }

        public bool Update(T model)
        {
            EntityEntry<T> entityEntry = Table.Update(model);
            return entityEntry.State == EntityState.Modified;
        }
    }
}
