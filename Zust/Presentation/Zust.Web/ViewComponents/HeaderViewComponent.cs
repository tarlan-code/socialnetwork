using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Zust.Application.Abstractions.Services;
using Zust.Application.Extensions;
using Zust.Application.ViewModels;
using Zust.Domain.Entities;

namespace Zust.Web.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        INotificationService _notificationService;
        IFriendService _friendService;
        IMapper _mapper;
        UserManager<AppUser> _userManager;

        public HeaderViewComponent(INotificationService notificationService, IFriendService friendService, UserManager<AppUser> userManager, IMapper mapper)
        {
            _notificationService = notificationService;
            _friendService = friendService;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            HeaderVM header = new();
            AppUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            header = _mapper.Map<HeaderVM>(user);

            List<Notification> notifications = await _notificationService.GetNotifications(user.Id);
            notifications = notifications.Where(n => n.IsReaded == false).ToList();
            List<NotificationVM> notificationVMs = new();
            NotificationVM notfVM = new();
            foreach (Notification notification in notifications)
            {
                notfVM = _mapper.Map<NotificationVM>(notification);
                notfVM.Date = notification.Date.DateDifference(DateTime.Now);
                notificationVMs.Add(notfVM);
            }
            header.Notifications = notificationVMs;

            List<AppUser> requests = _friendService.GetUnconfirmFriends(user.Id,0);

            List<RequestVM> requestusers = new();
            foreach (AppUser us in requests)
            {
                RequestVM requestvm = _mapper.Map<RequestVM>(us);
                requestvm.FriendsCount = await _friendService.GetFriendsCount(us.Id);
                requestusers.Add(requestvm);
            }
            header.Requests = requestusers;

            return View(header); 
        }


    }
}
