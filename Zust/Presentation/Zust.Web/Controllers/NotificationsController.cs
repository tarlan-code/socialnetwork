using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Zust.Application.Abstractions.Services;
using Zust.Application.Extensions;
using Zust.Application.ViewModels;
using Zust.Domain.Entities;

namespace Zust.Web.Controllers
{
    [Authorize]
    [Authorize(Roles = "Member")]

    public class NotificationsController : Controller
    {
        INotificationService _notificationService;
        IMapper _mapper;
        UserManager<AppUser> _userManager;

        public NotificationsController(INotificationService notificationService, UserManager<AppUser> userManager, IMapper mapper)
        {
            _notificationService = notificationService;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            
            return View(await GetNotfs());
        }

        [HttpPost]
        public async Task<IActionResult> GetNotifications(int skip=0)
        {
            
            return Json(await GetNotfs(skip));
        }

        async Task<List<NotificationVM>> GetNotfs(int skip = 0)
        {
            AppUser user = await _userManager.GetUserAsync(User);
            List<Notification> notifications = await _notificationService.GetNotifications(user.Id, skip);
            List<NotificationVM> notificationVMs = new();
            NotificationVM notfVM = new();
            foreach (Notification notification in notifications)
            {
                notfVM = _mapper.Map<NotificationVM>(notification);
                notfVM.Date = notification.Date.DateDifference(DateTime.Now);
                notificationVMs.Add(notfVM);
            }
            return notificationVMs;
        }

        public async Task<IActionResult> Read()
        {
            AppUser user = await _userManager.GetUserAsync(User);
            await _notificationService.Read(user.Id);
            return Ok();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Notification notf = await _notificationService.GetNotification((int)id);
            if (notf is null) return NotFound();
            AppUser user = await _userManager.GetUserAsync(User);
            if (user.Id != notf.ReceiverId) return BadRequest();
            await _notificationService.Delete(notf);
            return Ok();
        }


        public async Task<IActionResult> DeleteAll()
        {
            AppUser user = await _userManager.GetUserAsync(User);
            await _notificationService.Deleteall(user.Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
