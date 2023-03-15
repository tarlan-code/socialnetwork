using Microsoft.EntityFrameworkCore;
using Zust.Application.Abstractions.Services;
using Zust.Application.UnitOfWorks;
using Zust.Domain.Entities;

namespace Zust.Persistence.Services
{
    public class NotificationService:INotificationService
    {
        IUnitOfWork _unitOfWork;

        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddNotificationAsync(Notification notf)
        {
             bool result = await _unitOfWork.GetWriteRepository<Notification>().AddAsync(notf);
            await _unitOfWork.SaveAsync();
            return result;
        }

        public async Task<List<Notification>> GetNotifications(string ReceiverId, int skip = 0)
         => _unitOfWork.GetReadRepository<Notification>().GetAll(n => n.ReceiverId == ReceiverId, n => n.Sender).OrderByDescending(n=>n.Date).Skip(skip).Take(4).ToList();

        public async Task Read(string userId)
        {
            List<Notification> notfs = await GetNotifications(userId);
            notfs = notfs.Where(n => n.IsReaded == false).ToList();
            foreach (var item in notfs)
            {
                item.IsReaded = true;
            }
            await _unitOfWork.SaveAsync();
        }


        public async Task<Notification> GetNotification(int id)
        => await _unitOfWork.GetReadRepository<Notification>().GetAsync(n => n.Id == id); 

        public async Task Delete(Notification notification)
        {
            _unitOfWork.GetWriteRepository<Notification>().Delete(notification);
            await _unitOfWork.SaveAsync();
        }

        public async Task Deleteall(string userId)
        {
            List<Notification> notfs = await _unitOfWork.GetReadRepository<Notification>().GetAll(n => n.ReceiverId == userId).ToListAsync();
            _unitOfWork.GetWriteRepository<Notification>().DeleteRange(notfs);
            await _unitOfWork.SaveAsync();
        }
    }
}
