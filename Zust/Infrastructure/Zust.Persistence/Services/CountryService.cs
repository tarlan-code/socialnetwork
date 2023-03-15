using Zust.Application.Abstractions.Services;
using Zust.Application.UnitOfWorks;
using Zust.Domain.Entities;
using Zust.Persistence.UnitOfWorks;

namespace Zust.Persistence.Services
{
    internal class CountryService : ICountryService
    {
        IUnitOfWork _unitOfWork;

        public CountryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> CountryAnyAsync(int id)
        => _unitOfWork.GetReadRepository<Country>().AnyAsync(x => x.Id == id);

        public List<Country> GetAllCountry()
        {
            return _unitOfWork.GetReadRepository<Country>().GetAll().ToList();
        }
    }
}
