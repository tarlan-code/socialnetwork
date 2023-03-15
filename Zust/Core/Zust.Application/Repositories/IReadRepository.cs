using System.Linq.Expressions;
using Zust.Domain.Entities;

namespace Zust.Application.Repositories
{
    public interface IReadRepository<T> : IRepository<T> where T : class, IBaseEntity,new()
    {
        IQueryable<T> GetAll(Expression<Func<T, bool>>? method = null,params Expression<Func<T, object>>[] InlcludeParams);
        Task<T> GetAsync(Expression<Func<T,bool>> method,params Expression<Func<T, object>>[] InlcludeParams);
        Task<bool> AnyAsync(Expression<Func<T, bool>> method);
        Task<int> CountAsync(Expression<Func<T, bool>>? method = null);

    }
}
