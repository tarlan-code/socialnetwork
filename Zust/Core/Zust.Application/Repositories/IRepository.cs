using Microsoft.EntityFrameworkCore;
using Zust.Domain.Entities;

namespace Zust.Application.Repositories
{
    public interface IRepository<T> where T : class, IBaseEntity, new()
    {
        DbSet<T> Table { get; }
    }
}
