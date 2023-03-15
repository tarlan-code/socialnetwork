using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Zust.Application.Extensions;
using Zust.Domain.Entities;

namespace Zust.Web.Hubs
{
    public class ChatHub:Hub
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ChatHub( IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task OnConnectedAsync()
        {
            string username =  _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            Context.ConnectionId.AddUser(username);
            await Clients.All.SendAsync("isonline",ChatExtension.ConnectedUsers);
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Context.ConnectionId.RemoveUser();
            string username = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            await Clients.All.SendAsync("isoffline", username);
        }
    }
}
