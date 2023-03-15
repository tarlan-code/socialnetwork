using Zust.Application.Abstractions.Services;
using Zust.Application.UnitOfWorks;
using Zust.Domain.Entities;

namespace Zust.Persistence.Services
{
    public class PrivacySettingService : IPrivacySettingService
    {
        IUnitOfWork _unitOfWork;

        public PrivacySettingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddPrivacySetting(PrivacySetting privacySetting)
        {
            bool result = await _unitOfWork.GetWriteRepository<PrivacySetting>().AddAsync(privacySetting);
            await _unitOfWork.SaveAsync();
            return result;
        }

        public async Task<PrivacySetting> GetPrivacySettingAsync(string userId)
            =>await _unitOfWork.GetReadRepository<PrivacySetting>().GetAsync(s => s.AppUserId == userId);

        public bool UpdatePrivacySetting(PrivacySetting privacy)
        {

         bool result = _unitOfWork.GetWriteRepository<PrivacySetting>().Update(privacy);
            _unitOfWork.Save();
            return result;
        }
    }
}
