using Zust.Domain.Entities;

namespace Zust.Application.Abstractions.Services
{
    public interface IBirthdayService
    {
        Task<List<AppUser>> TodayBirthdays(List<AppUser> friends);
        Task<List<AppUser>> ComingBirthdays(List<AppUser> friends);
    }
}
