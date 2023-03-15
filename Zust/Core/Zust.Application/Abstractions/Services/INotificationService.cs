using Zust.Application.UnitOfWorks;
using Zust.Domain.Entities;

namespace Zust.Application.Abstractions.Services
{
    public interface INotificationService
    {
       
        Task<bool> AddNotificationAsync(Notification notf);
        Task<List<Notification>> GetNotifications(string ReceiverId,int skip = 0);
        Task Read(string userId);
        Task<Notification> GetNotification(int id);
        Task Delete(Notification notification);
        Task Deleteall(string userId);
    }
}
