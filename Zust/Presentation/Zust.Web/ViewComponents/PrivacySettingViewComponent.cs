using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Zust.Application.Abstractions.Services;
using Zust.Application.ViewModels;
using Zust.Domain.Entities;

namespace Zust.Web.ViewComponents
{
    public class PrivacySettingViewComponent:ViewComponent
    {
        IPrivacySettingService _privacyService;
        IMapper _mapper;
        UserManager<AppUser> _userManager;
        public PrivacySettingViewComponent(IPrivacySettingService privacyService, UserManager<AppUser> userManager, IMapper mapper)
        {
            _privacyService = privacyService;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            AppUser user = await _userManager.GetUserAsync(HttpContext.User);
            PrivacySetting privacy = await _privacyService.GetPrivacySettingAsync(user.Id);
            PrivacyVM privacyVM = _mapper.Map<PrivacyVM>(privacy);
            return View(privacyVM);
        }
    }
}
