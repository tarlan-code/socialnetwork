using Zust.Application.Abstractions.Services;
using Zust.Application.Extensions;
using Zust.Application.UnitOfWorks;
using Zust.Domain.Entities;

namespace Zust.Persistence.Services
{
    public class MessageService : IMessageService
    {
        IUnitOfWork _unitOfWork;
        IPathService _pathService;

        public MessageService(IUnitOfWork unitOfWork, IPathService pathService)
        {
            _unitOfWork = unitOfWork;
            _pathService = pathService;
        }

        public async Task<bool> AddMessageAsync(Message message)
        {
            bool result = await _unitOfWork.GetWriteRepository<Message>().AddAsync(message);
            await _unitOfWork.SaveAsync();
            return result;
        }

        public async Task DeleteAllMessages(AppUser user1, AppUser user2)
        {
            List<Message> messages = await GetMessagesAsync(user1.Id, user2.Id);
            foreach (Message message in messages)
            {
                if(message.Media is not null)
                {
                    message.Media.Delete(Path.Combine(_pathService.UsersFolder, user1.UserName, "messages"));
                    message.Media.Delete(Path.Combine(_pathService.UsersFolder, user2.UserName, "messages"));
                }
            }

            _unitOfWork.GetWriteRepository<Message>().DeleteRange(messages);
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<Message>> GetMessagesAsync(string SenderId,string ReceiverId)
        =>_unitOfWork.GetReadRepository<Message>().GetAll(m => (m.SenderId == SenderId && m.ReceiverId == ReceiverId) || (m.SenderId == ReceiverId && m.ReceiverId ==SenderId )).ToList();
        
    }
}
