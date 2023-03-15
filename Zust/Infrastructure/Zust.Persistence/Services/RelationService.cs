using Zust.Application.Abstractions.Services;
using Zust.Application.UnitOfWorks;
using Zust.Domain.Entities;

namespace Zust.Persistence.Services
{
    public class RelationService : IRelationService
    {
        IUnitOfWork _unitOfWork;

        public RelationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Relation> GetAllRelations()
        =>_unitOfWork.GetReadRepository<Relation>().GetAll().ToList();

        public Task<bool> RelationAnyAsync(int id)
        => _unitOfWork.GetReadRepository<Relation>().AnyAsync(x => x.Id == id);
    }
}
