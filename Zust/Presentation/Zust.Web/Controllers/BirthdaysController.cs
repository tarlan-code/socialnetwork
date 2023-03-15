using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Zust.Application.Abstractions.Services;
using Zust.Application.ViewModels;
using Zust.Domain.Entities;

namespace Zust.Web.Controllers
{
    [Authorize]
    [Authorize(Roles = "Member")]
    public class BirthdaysController : Controller
    {
        UserManager<AppUser> _userManager;
        IFriendService _friendService;
        IBirthdayService _birthdayService;
        IMapper _mapper;

        public BirthdaysController(UserManager<AppUser> userManager, IBirthdayService birthdayService, IFriendService friendService, IMapper mapper)
        {
            _userManager = userManager;
            _birthdayService = birthdayService;
            _friendService = friendService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            BirthdayPageVM birthdayPage  = new();
            AppUser user = await _userManager.GetUserAsync(User);
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
