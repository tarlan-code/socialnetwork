using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Zust.Application.Abstractions.Services;
using Zust.Application.ViewModels;
using Zust.Domain.Entities;

namespace Zust.Web.Controllers
{
    [Authorize]
    [Authorize(Roles = "Member")]
    public class RequestsController : Controller
    {
        UserManager<AppUser> _userManager;
        IFriendService _friendService;
        IUserService _userService;
        IPrivacySettingService _privacy;
        IToastNotification _notf;
        IMapper _mapper;

        public RequestsController(UserManager<AppUser> userManager, IFriendService friendService, IToastNotification notf, IMapper mapper, IUserService userService, IPrivacySettingService privacy)
        {
            _userManager = userManager;
            _friendService = friendService;
            _notf = notf;
            _mapper = mapper;
            _userService = userService;
            _privacy = privacy;
        }

        public async Task<IActionResult> Index()
        {
           
            return View(await Requests());
        }

        public async Task<IActionResult> GetRequests(int skip=0)
        {
            
            return Json(await Requests(skip));
        }

        public async Task<List<RequestVM>> Requests(int skip=0)
        {
            AppUser user = await _userManager.GetUserAsync(User);
            List<AppUser> requests = _friendService.GetUnconfirmFriends(user.Id, skip);

            List<RequestVM> requestusers = new();
            foreach (AppUser us in requests)
            {
                RequestVM requestvm = _mapper.Map<RequestVM>(us);
                requestvm.FriendsCount = await _friendService.GetFriendsCount(us.Id);
                requestusers.Add(requestvm);
            }
            return requestusers;
        }
       

        public async Task<IActionResult> AddFriend(string? username)
        {
            if (username is null) return BadRequest();
            AppUser user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound();

            AppUser IdenUser = await _userManager.GetUserAsync(User);

            PrivacySetting privacy = await _privacy.GetPrivacySettingAsync(user.Id);
            if (privacy.WhoCanSeeYourProfile is null) return BadRequest();

            if(privacy.WhoCanSendFollow is null)
            {
                _notf.AddAlertToastMessage("User Privacy No one send request", new ToastrOptions
                {
                    Title = "Not send request"
                });
                return Content("False");
            }
            else if (privacy.WhoCanSendFollow == false)
            {
                bool isFriendsOfFriend = _friendService.FriendsOfFriends(user.Id).Contains(IdenUser.Id);
                if (!isFriendsOfFriend)
                {
                    _notf.AddAlertToastMessage("User Privacy Friends of friends follow", new ToastrOptions
                    {
                        Title ="Not send request"
                    });
                    return Content("False");
                }
            }
           
            bool result = await _friendService.CheckUnique(IdenUser.Id, user.Id);
            if (!result)
            {
                await _friendService.AddFriend(IdenUser.Id,user.Id);
                return Content(result.ToString());
            }

            return Content(result.ToString());
        }


        public async Task<IActionResult> RemoveFriend(string? username)
        {
            if (username is null) return BadRequest();
            AppUser user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound();
            AppUser IdenUser = await _userManager.GetUserAsync(User);
            await _friendService.RemoveFreind(user.Id, IdenUser.Id);

            return Content("True");
        }


        public async Task<IActionResult> Confirm(string? username)
        {
            if (username is null) return BadRequest();
            AppUser user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound();
            AppUser IdenUser = await _userManager.GetUserAsync(User);
            Friend friend = await _friendService.GetFriend(user.Id, IdenUser.Id);
            if (friend is not null) friend.IsConfirmed = true;
            await _friendService.SaveFriendAsync();
            return RedirectToAction("Index", "Profiles", new {username = username});
        }
    }
}
