using Zust.Application.Abstractions.Services;
using Zust.Application.UnitOfWorks;
using Zust.Domain.Entities;

namespace Zust.Persistence.Services
{
    internal class GenderService : IGenderService
    {
        IUnitOfWork _unitOfWork;

        public GenderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> GenderAnyAsync(int id)
        => _unitOfWork.GetReadRepository<Gender>().AnyAsync(x => x.Id == id);

        public List<Gender> GetAllGender()
        => _unitOfWork.GetReadRepository<Gender>().GetAll().ToList();
        

       
    }
}
