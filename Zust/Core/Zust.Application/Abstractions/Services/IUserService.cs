using Zust.Domain.Entities;

namespace Zust.Application.Abstractions.Services
{
    public interface IUserService
    {
        List<AppUser> GetSearchedUsers(string username,string q);
        IQueryable<AppUser> GetAllUsers();
        Task<AppUser> GetIdentityUserInfo(string username);
        Task<AppUser> GetIdentityUserHeaderInfo(string id);
        Task<List<AppUser>> GetRequestsUser(List<string> UserIds);
        Dictionary<string, string> GetUsersWithUsername(List<string> userIds);
        Task<List<AppUser>> GetDeletedUsers();
        Task DeleteUser(AppUser user);
    }
}
