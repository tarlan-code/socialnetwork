using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Zust.Application.Abstractions.Services;
using Zust.Application.ViewModels;
using Zust.Domain.Entities;

namespace Zust.Web.Areas.Manage.Controllers
{
    [Area(nameof(Manage))]
    [Authorize (Roles ="Admin")]
    public class UsersController : Controller
    {
        IUserService _userService;
        IMapper _mapper;
        IToastNotification _notf;
        UserManager<AppUser> _userManager;

        public UsersController(IUserService userService, IMapper mapper, UserManager<AppUser> userManager, IToastNotification notf)
        {
            _userService = userService;
            _mapper = mapper;
            _userManager = userManager;
            _notf = notf;
        }

        public async Task<IActionResult> Index()
        {
            List<AppUser> users = await _userService.GetDeletedUsers();
            List<PanelUserListVM> deletedUsers = new();
            users.ForEach(u => deletedUsers.Add(_mapper.Map<PanelUserListVM>(u)));

            return View(deletedUsers);
        }


        public async Task<IActionResult> Delete(string id)
        {
            if (String.IsNullOrEmpty(id)) return BadRequest();
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();
            if(user.IsDeleted == false || user.DeletedDate < DateTime.Now)
            {
                _notf.AddErrorToastMessage("User not delete account");
                return RedirectToAction(nameof(Index));
            }
            await _userService.DeleteUser(user);
            
            return RedirectToAction(nameof(Index));
        }


    }
}
