using Zust.Domain.Entities;

namespace Zust.Application.Abstractions.Services
{
    public interface ICityService
    {
        List<City> GetCities(int countryId);
        Task<bool> CityAnyAsync(int id, int countryId);
    }
}
