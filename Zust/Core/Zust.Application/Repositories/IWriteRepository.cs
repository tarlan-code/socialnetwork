using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Zust.Domain.Entities;

namespace Zust.Application.Repositories
{
    public interface IWriteRepository<T>:IRepository<T> where T : class,IBaseEntity,new()
    {
        Task<bool> AddAsync(T model);
        Task<bool> AddRangeAsync(List<T> models);
        Task<bool> Delete(Expression<Func<T,bool>> method);
        bool Delete(T model);
        bool DeleteRange(List<T> models);
        bool Update(T model);
    }
}
