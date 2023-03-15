using Zust.Domain.Entities;

namespace Zust.Application.Abstractions.Services
{
    public interface IEventService
    {
        Task<int> SaveEventAsync(Event eventModel);
        Task<bool> CheckEventCategory(int id);
        Task<List<EventCategory>> GetEventCategories();
        Task<List<Event>> GetEventsAsync(int skip = 0, string? q=null);
        Task<Event> GetEventAsync(int id);
        Task UpdateAttend(Event AttendedEvent);
        Task<bool> RemoveAttend(EventAttend eventAttend);
        Task DeleteEvent(Event deletedEvent);
    }
}
