using Microsoft.EntityFrameworkCore;
using Zust.Application.Abstractions.Services;
using Zust.Application.Extensions;
using Zust.Application.UnitOfWorks;
using Zust.Domain.Entities;

namespace Zust.Persistence.Services
{
    internal class UserService : IUserService
    {
        IUnitOfWork _unitOfWork;
        IFriendService _friendService;
        IEventService _eventService;
        public UserService(IUnitOfWork unitOfWork, IFriendService friendService, IEventService eventService)
        {
            _unitOfWork = unitOfWork;
            _friendService = friendService;
            _eventService = eventService;
        }

        public IQueryable<AppUser> GetAllUsers()
            =>_unitOfWork.GetReadRepository<AppUser>().GetAll();

        public List<AppUser> GetSearchedUsers(string username,string q)
        {
           return _unitOfWork.GetReadRepository<AppUser>()
                .GetAll(u => u.UserName != username && (u.PrivacySetting.WhoCanSeeYourProfile==true || u.PrivacySetting.WhoCanSeeYourProfile==false) &&  (u.Name.Contains(q) || u.UserName.Contains(q) && u.IsDeleted == false),u=>u.PrivacySetting).
                Take(5)
                .ToList();
        }

        public Task<AppUser> GetIdentityUserHeaderInfo(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AppUser> GetIdentityUserInfo(string username)
        {
            AppUser user = await _unitOfWork.GetReadRepository<AppUser>().GetAsync(u=>u.UserName == username,u=>u.City,u=>u.Country,u=>u.Gender,u=>u.Relation,u=>u.PrivacySetting);
            return user;
        }

        public async Task<List<AppUser>> GetRequestsUser(List<string> UserIds)
            =>await GetAllUsers().Where(u => UserIds.Contains(u.Id)).ToListAsync();

        public Dictionary<string,string> GetUsersWithUsername(List<string> userIds)
        => GetAllUsers().Where(u => userIds.Contains(u.Id)).ToDictionary(s => s.UserName, s => s.Id);

        public async Task<List<AppUser>> GetDeletedUsers()
        {
            return await _unitOfWork.GetReadRepository<AppUser>().GetAll(u =>  u.IsDeleted == true && u.DeletedDate >= DateTime.Now).ToListAsync();
       
        }

        public async Task DeleteUser(AppUser user)
        {
            List<Friend> friends = await _unitOfWork.GetReadRepository<Friend>().GetAll(f => f.Sender == user || f.Receiver == user).ToListAsync();
            _unitOfWork.GetWriteRepository<Friend>().DeleteRange(friends);
            List<EventAttend> eventAttends = await _unitOfWork.GetReadRepository<EventAttend>().GetAll(x => x.AppUser == user).ToListAsync();
            _unitOfWork.GetWriteRepository<EventAttend>().DeleteRange(eventAttends);
            _unitOfWork.Save();
            List<Event> events = await _unitOfWork.GetReadRepository<Event>().GetAll(e => e.AppUser == user).ToListAsync();

            foreach (var item in events)
            {
                List<EventAttend> ea = await _unitOfWork.GetReadRepository<EventAttend>().GetAll(ea => ea.EventId == item.Id).ToListAsync();
                _unitOfWork.GetWriteRepository<EventAttend>().DeleteRange(ea);
            }

            _unitOfWork.GetWriteRepository<Event>().DeleteRange(events);
            List<PostReaction> reactions = await _unitOfWork.GetReadRepository<PostReaction>().GetAll(e => e.AppUser == user).ToListAsync();
            _unitOfWork.GetWriteRepository<PostReaction>().DeleteRange(reactions);
            List<Message> messages = await _unitOfWork.GetReadRepository<Message>().GetAll(e => e.Sender == user || e.Receiver == user).ToListAsync();
            _unitOfWork.GetWriteRepository<Message>().DeleteRange(messages);

            List<Comment> comments = await _unitOfWork.GetReadRepository<Comment>().GetAll(e => e.AppUser == user).ToListAsync();
            _unitOfWork.GetWriteRepository<Comment>().DeleteRange(comments);
            _unitOfWork.GetWriteRepository<AppUser>().Delete(user);
            await _unitOfWork.SaveAsync();
        }
    }
}
