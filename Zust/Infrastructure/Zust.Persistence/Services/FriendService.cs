using Microsoft.EntityFrameworkCore;
using Zust.Application.Abstractions.Services;
using Zust.Application.UnitOfWorks;
using Zust.Domain.Entities;

namespace Zust.Persistence.Services
{
    public class FriendService : IFriendService
    {
        IUnitOfWork _unitOfWork;

        public FriendService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<Friend> GetFriends(string userId)
            => _unitOfWork.GetReadRepository<Friend>()
            .GetAll(f => (f.SenderId == userId || f.ReceiveId == userId) && f.IsConfirmed == true);
        public IQueryable<AppUser> GetYourFriends(string userId,string? q)
        {
            if(String.IsNullOrEmpty(q))
            return _unitOfWork.GetReadRepository<Friend>()
            .GetAll(f => (f.SenderId == userId || f.ReceiveId == userId) && f.IsConfirmed == true,f=>f.Sender,f=>f.Receiver).Select(u=>u.Sender.Id != userId ? u.Sender : u.Receiver);

            return _unitOfWork.GetReadRepository<Friend>()
            .GetAll(f => (f.SenderId == userId || f.ReceiveId == userId) && f.IsConfirmed == true && (f.Sender.UserName.Contains(q) || f.Receiver.UserName.Contains(q) || f.Sender.Name.Contains(q) || f.Receiver.Name.Contains(q))).Select(u => u.Sender.Id != userId ? u.Sender : u.Receiver);

        }

        public List<AppUser> GetUnconfirmFriends(string userId,int skip=0)
            => _unitOfWork.GetReadRepository<Friend>()
            .GetAll(f => f.ReceiveId == userId && f.IsConfirmed == false).Select(f => f.Sender).Skip(skip).Take(1).ToList();
        public async Task<bool> AddFriend(string SenderId,string ReceiverId)
        {
            bool result  = await _unitOfWork.GetWriteRepository<Friend>().
            AddAsync(new Friend { SenderId = SenderId,ReceiveId = ReceiverId,IsConfirmed=false});
            await _unitOfWork.SaveAsync();
            return result;
        }

        public async Task<bool> CheckUnique(string SenderId, string ReceiverId)
            => await _unitOfWork.GetReadRepository<Friend>().AnyAsync(f=>(f.SenderId == SenderId && f.ReceiveId == ReceiverId) || (f.ReceiveId == SenderId && f.SenderId == ReceiverId));



        public async Task<bool> IsFriend(string SenderId, string ReceiverId)
        {
            if (SenderId == ReceiverId) return true;
            return await _unitOfWork.GetReadRepository<Friend>().AnyAsync(f => ((f.SenderId == SenderId && f.ReceiveId == ReceiverId) || (f.ReceiveId == SenderId && f.SenderId == ReceiverId)) && f.IsConfirmed == true);
            
        }

        public async Task RemoveFreind(string SenderId, string ReceiverId)
        {
            Friend friend = await GetFriend(SenderId, ReceiverId);
            if (friend is not null)
            {
                _unitOfWork.GetWriteRepository<Friend>().Delete(friend);
                 await _unitOfWork.SaveAsync();
            }
        }

        public async Task<Friend> GetFriend(string SenderId, string ReceiverId)
            =>await _unitOfWork.GetReadRepository<Friend>()
            .GetAsync(f => (f.SenderId == SenderId && f.ReceiveId == ReceiverId) || (f.ReceiveId == SenderId && f.SenderId == ReceiverId));

        public async Task<bool?> CheckFriend(string SenderId, string ReceiverId)
        {
            Friend friend = await GetFriend(SenderId, ReceiverId);

            if (friend is null) return null;
            else if(friend.SenderId == SenderId && friend.IsConfirmed == false) return false;
            else if(friend.ReceiveId == SenderId && friend.IsConfirmed == false) return true;
            return null;
        }


        public async Task SaveFriendAsync()
        {
            await _unitOfWork.SaveAsync();
        }

        public async Task<int> GetFriendsCount(string userId)
            => await _unitOfWork.GetReadRepository<Friend>()
            .CountAsync(f => (f.SenderId == userId || f.ReceiveId == userId) && f.IsConfirmed == true);

        public List<string> GetFriendsList(string userId)
        {
            List<string> friends = GetFriends(userId).Where(s => s.SenderId != userId).Select(u => u.SenderId).ToList();
            friends.AddRange(GetFriends(userId).Where(s => s.ReceiveId != userId).Select(u => u.ReceiveId).ToList());
            return friends;
        }
        public List<string> FriendsOfFriends(string userId)
        {
            List<string> friends = GetFriendsList(userId);
            List<string> friendsoffriends = new();
            foreach (string friendId in friends)
            {
                friendsoffriends.AddRange(GetFriendsList(friendId));
            }
            return friendsoffriends;

        }

    }
}
