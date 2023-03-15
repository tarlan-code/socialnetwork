using Microsoft.EntityFrameworkCore;
using Zust.Application.Abstractions.Services;
using Zust.Application.Extensions;
using Zust.Application.UnitOfWorks;
using Zust.Domain.Entities;

namespace Zust.Persistence.Services
{
    public class EventService : IEventService
    {
        IUnitOfWork _unitOfWork;
        IPathService _pathService;

        public EventService(IUnitOfWork unitOfWork, IPathService pathService)
        {
            _unitOfWork = unitOfWork;
            _pathService = pathService;
        }

        public async Task<bool> CheckEventCategory(int id)
        {
            return await _unitOfWork.GetReadRepository<EventCategory>().AnyAsync(e => e.Id == id);
        }

        public async Task<List<EventCategory>> GetEventCategories()
        {
            return await _unitOfWork.GetReadRepository<EventCategory>().GetAll().ToListAsync();
        }

        public async Task<int> SaveEventAsync(Event eventModel)
        {
             await _unitOfWork.GetWriteRepository<Event>().AddAsync(eventModel);
            return await _unitOfWork.SaveAsync();

        }

        public async Task<List<Event>> GetEventsAsync(int skip, string? q=null)
        {
            if (String.IsNullOrEmpty(q))
            {
                return await _unitOfWork.GetReadRepository<Event>().GetAll(null, e => e.AppUser, e => e.EventAttends, e => e.EventCategory).Skip(skip).Take(4).ToListAsync();
            }
            else
            {
                 return await _unitOfWork.GetReadRepository<Event>().GetAll(e=>e.Name.Contains(q), e=>e.AppUser, e=>e.EventAttends,e=>e.EventCategory).Skip(skip).ToListAsync();

            }

        }

        public async Task<Event> GetEventAsync(int id)
         => await _unitOfWork.GetReadRepository<Event>().GetAsync(e=>e.Id == id ,e=>e.EventAttends,e=>e.EventCategory);
     
        

        public async Task UpdateAttend(Event AttendedEvent)
        {
            _unitOfWork.GetWriteRepository<Event>().Update(AttendedEvent);
            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> RemoveAttend(EventAttend eventAttend)
        {
            bool result = _unitOfWork.GetWriteRepository<EventAttend>().Delete(eventAttend);
            await _unitOfWork.SaveAsync();
            return result;
        }

        public async Task DeleteEvent(Event deletedEvent)
        {
            _unitOfWork.GetWriteRepository<EventAttend>().DeleteRange(_unitOfWork.GetReadRepository<EventAttend>().GetAll(e => e.Event == deletedEvent).ToList());
            _unitOfWork.GetWriteRepository<Event>().Delete(deletedEvent);
            deletedEvent.Image.Delete(Path.Combine(_pathService.UsersFolder, deletedEvent.AppUser.UserName, "events"));
            await _unitOfWork.SaveAsync();
        }
    }
}
