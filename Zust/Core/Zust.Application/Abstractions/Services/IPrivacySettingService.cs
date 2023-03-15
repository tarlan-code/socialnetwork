using Zust.Domain.Entities;

namespace Zust.Application.Abstractions.Services
{
    public interface IPrivacySettingService
    {
        Task<PrivacySetting> GetPrivacySettingAsync(string userId);
        bool UpdatePrivacySetting(PrivacySetting privacy);
        Task<bool> AddPrivacySetting(PrivacySetting privacySetting);
    }
}
