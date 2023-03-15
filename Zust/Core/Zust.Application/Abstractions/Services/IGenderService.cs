using Zust.Domain.Entities;

namespace Zust.Application.Abstractions.Services
{
    public interface IGenderService
    {
        List<Gender> GetAllGender();
        Task<bool> GenderAnyAsync(int id);
    }
}
