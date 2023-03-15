using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zust.Application.Abstractions.Services;
using Zust.Application.UnitOfWorks;
using Zust.Domain.Entities;

namespace Zust.Persistence.Services
{
    public class CityService : ICityService
    {
        IUnitOfWork _unitOfWork;

        public CityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> CityAnyAsync(int id, int countryId)
        => _unitOfWork.GetReadRepository<City>().AnyAsync(x => x.Id == id && x.CountryId == countryId);

        public List<City> GetCities(int countryId)
            =>_unitOfWork.GetReadRepository<City>().GetAll(c=>c.CountryId == countryId).ToList();
        
    }
}
