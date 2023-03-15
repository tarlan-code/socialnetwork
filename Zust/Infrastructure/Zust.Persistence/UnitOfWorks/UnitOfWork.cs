using Zust.Application.Repositories;
using Zust.Application.UnitOfWorks;
using Zust.Domain.Entities;
using Zust.Persistence.Contexts;
using Zust.Persistence.Repositories;

namespace Zust.Persistence.UnitOfWorks
{
    internal class UnitOfwork : IUnitOfWork
    {

        readonly AppDbContext _context;
        
        public UnitOfwork(AppDbContext context)
        {
            _context = context;
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        public IReadRepository<T> GetReadRepository<T>() where T : class,IBaseEntity, new()
            =>new ReadRepository<T>(_context);
        

        public IWriteRepository<T> GetWriteRepository<T>() where T : class, IBaseEntity, new()
        => new WriteRepository<T>(_context);

        public int Save()
            =>_context.SaveChanges();

        public async Task<int> SaveAsync()
            =>await _context.SaveChangesAsync();
        
    }
}
