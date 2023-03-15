using Zust.Domain.Entities;

namespace Zust.Application.Abstractions.Services
{
    public interface IMessageService
    {
        Task<List<Message>> GetMessagesAsync(string SenderId, string ReceiverId);
        Task<bool> AddMessageAsync(Message message);
        Task DeleteAllMessages(AppUser user1, AppUser user2);
    }
}
