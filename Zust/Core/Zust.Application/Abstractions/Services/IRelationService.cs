using Zust.Domain.Entities;

namespace Zust.Application.Abstractions.Services
{
    public interface IRelationService
    {
        List<Relation> GetAllRelations();
        Task<bool> RelationAnyAsync(int id);
    }
}
