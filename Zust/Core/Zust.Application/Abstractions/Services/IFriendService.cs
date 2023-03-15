using Zust.Domain.Entities;

namespace Zust.Application.Abstractions.Services
{
    public interface IFriendService
    {
        IQueryable<Friend> GetFriends(string userId);
        IQueryable<AppUser> GetYourFriends(string userId,string q);
        Task<bool> AddFriend(string SenderId, string ReceiverId);
        Task<bool> CheckUnique(string SenderId, string ReceiverId);
        Task<bool> IsFriend(string SenderId,string ReceiverId);
        Task RemoveFreind(string SenderId, string ReceiverId);
        Task<Friend> GetFriend(string SenderId, string ReceiverId);
        Task<bool?> CheckFriend(string SenderId, string ReceiverId);
        Task<int> GetFriendsCount(string userId);
        Task SaveFriendAsync();
        List<AppUser> GetUnconfirmFriends(string userId,int skip=0);
        List<string> FriendsOfFriends(string userId);
        List<string> GetFriendsList(string userId);

    }
}
