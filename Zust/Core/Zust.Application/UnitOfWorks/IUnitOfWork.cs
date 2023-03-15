using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zust.Application.Repositories;
using Zust.Domain.Entities;

namespace Zust.Application.UnitOfWorks
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IReadRepository<T> GetReadRepository<T>() where T : class, IBaseEntity, new();
        IWriteRepository<T> GetWriteRepository<T>() where T :class, IBaseEntity, new();
        Task<int> SaveAsync();
        int Save();

    }
}
