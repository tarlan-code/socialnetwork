using Zust.Domain.Entities;

namespace Zust.Application.Abstractions.Services
{
    public interface ICountryService
    {
       List<Country> GetAllCountry();
       Task<bool> CountryAnyAsync(int id);
    }
}
