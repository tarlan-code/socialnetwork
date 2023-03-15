using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Zust.Application.Abstractions.Services;
using Zust.Application.Extensions;
using Zust.Application.ViewModels;
using Zust.Domain.Entities;
using Zust.Web.Hubs;

namespace Zust.Web.Controllers
{
    [Authorize]
    [Authorize(Roles = "Member")]

    public class MessagesController : Controller
    {
        IFriendService _friendService;
        IMessageService _messageService;
        IMapper _mapper;
        IToastNotification _notf;
        IPathService _pathService;
        INotificationService _notificationService;
        IHubContext<ChatHub> _hub;
        UserManager<AppUser> _userManager;

        public MessagesController(IFriendService friendService, UserManager<AppUser> userManager, IMapper mapper, IMessageService messageService, IToastNotification notf, IPathService pathService, IHubContext<ChatHub> hub, INotificationService notificationService)
        {
            _friendService = friendService;
            _userManager = userManager;
            _mapper = mapper;
            _messageService = messageService;
            _notf = notf;
            _pathService = pathService;
            _hub = hub;
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Index(string username="")
        {
            if (username == "") return View();

            AppUser user = await _userManager.FindByNameAsync(username);
            AppUser IdenUser = await _userManager.GetUserAsync(User);
            ChatVM chat = _mapper.Map<ChatVM>(user);

            List<Message> messages = await _messageService.GetMessagesAsync(user.Id, IdenUser.Id);
            List<MessageVM> messageVMs = new();

            messages.ForEach(m => messageVMs.Add(_mapper.Map<MessageVM>(m)));

            chat.Messages = messageVMs;
            return Json(chat);
        }



        public async Task<IActionResult> GetContacts(string q="")
        {
            AppUser user = await _userManager.GetUserAsync(User);
            List<AppUser> friends = await _friendService.GetYourFriends(user.Id,q).Take(5).ToListAsync();
            List<ContactVM> contacts = new();
            friends.ForEach(f => contacts.Add(_mapper.Map<ContactVM>(f)));
            return Json(contacts);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(SendedMessageVM message,string username)
        {
            if(username is null || username == "undefined")
            {
                _notf.AddWarningToastMessage("Please select contact user");
                return Content("False");
            }

            AppUser user = await _userManager.FindByNameAsync(username);
            if(user is null)
            {
                _notf.AddErrorToastMessage("User not Found");
                return Content("False");
            }
            IFormFile? file = message?.File;

            if(file is not null)
            {
                string result = file.CheckValidate(50, "image", "video");
                if(result.Length > 0)
                    ModelState.AddModelError("Image", result);
            }

            if(message.File is null && message.Content is null)
            {
                _notf.AddWarningToastMessage("Message is not empty");
                return Content("False");
            }
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState)
                {
                    if (item.Value.Errors.Count > 0)
                        _notf.AddWarningToastMessage(item.Value.Errors.FirstOrDefault().ErrorMessage.ToString(), new ToastrOptions
                        {
                            Title = item.Key
                        });
                }
                return Content("False");
            }



            Message newMessage = new();

            newMessage.Content = message.Content;
            newMessage.Receiver = user;
            AppUser IdenUser = await _userManager.GetUserAsync(User);
            newMessage.Sender = IdenUser;

            if(file is not null)
            {
                newMessage.Media = file.SaveFile(Path.Combine(_pathService.UsersFolder, IdenUser.UserName, "messages"));
            }

            bool addresult = await _messageService.AddMessageAsync(newMessage);
            if (!addresult) return Content("False");

            MessageVM ToViewMessage = _mapper.Map<MessageVM>(newMessage);

            string? connectionId = user.UserName.GetConnectionIdByUsername();
            if(connectionId is null)
            {
                    await _notificationService.AddNotificationAsync(new Notification
                    {
                        ReceiverId = user.Id,
                        Sender = IdenUser,
                        Title = "Send new Message"
                    });
            }
            else
            {
                await _hub.Clients.Client(connectionId).SendAsync("receiveMessage", ToViewMessage,IdenUser.UserName);
            }
            return Json(ToViewMessage);
        }


        public async Task<IActionResult> DeleteMessages(string username)
        {
            AppUser user1 = await _userManager.GetUserAsync(User);
            AppUser user2 = await _userManager.FindByNameAsync(username);
            if (user2 is null) return BadRequest();
            await _messageService.DeleteAllMessages(user1, user2);
            return RedirectToAction("Index","Messages");
        }
    }
}
