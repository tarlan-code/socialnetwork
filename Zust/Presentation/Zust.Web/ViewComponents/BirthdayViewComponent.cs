using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zust.Application.Abstractions.Services;
using Zust.Application.ViewModels;
using Zust.Domain.Entities;

namespace Zust.Web.ViewComponents
{
    public class BirthdayViewComponent:ViewComponent
    {
        UserManager<AppUser> _userManager;
        IFriendService _friendService;
        IBirthdayService _birthdayService;
        IMapper _mapper;
        IHttpContextAccessor _httpContextAccessor;


        public BirthdayViewComponent(UserManager<AppUser> userManager, IFriendService friendService, IBirthdayService birthdayService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _friendService = friendService;
            _birthdayService = birthdayService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            BirthdayPageVM birthdayPage = new();
            AppUser user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            List<AppUser> friends = await _friendService.GetYourFriends(user.Id, null).ToListAsync();
            List<BirthdayVM> commingBirthdays = new();
            foreach (AppUser friend in await _birthdayService.ComingBirthdays(friends))
            {
                commingBirthdays.Add(_mapper.Map<BirthdayVM>(friend));
            }
            birthdayPage.CommingBirthdays = commingBirthdays;

            List<BirthdayVM> TodayBirthdays = new();
            foreach (AppUser friend in await _birthdayService.TodayBirthdays(friends))
            {
                TodayBirthdays.Add(_mapper.Map<BirthdayVM>(friend));
            }
            birthdayPage.ToDayBirthDays = TodayBirthdays;


            return View(birthdayPage);
        }
    }
}
