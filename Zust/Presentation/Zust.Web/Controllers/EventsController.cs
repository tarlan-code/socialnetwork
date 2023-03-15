using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using Zust.Application.Abstractions.Services;
using Zust.Application.Extensions;
using Zust.Application.ViewModels;
using Zust.Domain.Entities;

namespace Zust.Web.Controllers
{
    [Authorize]
    [Authorize(Roles = "Member")]
    public class EventsController : Controller
    {
        IEventService _eventService;
        IMapper _mapper;
        IPathService _pathService;
        IToastNotification _notf;
        UserManager<AppUser> _userManager;

        public EventsController(IEventService eventService, IMapper mapper, UserManager<AppUser> userManager, IPathService pathService, IToastNotification notf)
        {
            _eventService = eventService;
            _mapper = mapper;
            _userManager = userManager;
            _pathService = pathService;
            _notf = notf;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Categories = new SelectList(await _eventService.GetEventCategories(), nameof(Event.Id), nameof(Event.Name));

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(EventVM eventVM)
        {
            IFormFile? image = eventVM.Image;
            if (image is not null)
            {
                string result = image.CheckValidate(5, "image");
                if (result.Length > 0)
                {
                    ModelState.AddModelError("Image", result);
                }
            }
            if(eventVM.EventDate < DateTime.Now.AddHours(6))
            {
                ModelState.AddModelError("EventDate", "Date minimum 6 hours after");
            }
            if(!await _eventService.CheckEventCategory(eventVM.EventCategoryId))
            {
                ModelState.AddModelError("EventCategoryId", "Wrong Category Id");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(await _eventService.GetEventCategories(), nameof(Event.Id), nameof(Event.Name));
                return View(eventVM);
            }

            Event newEvent = _mapper.Map<Event>(eventVM);

            AppUser user = await _userManager.GetUserAsync(User);

            newEvent.AppUser = user;

        
            newEvent.Image  = image.SaveFile(Path.Combine(_pathService.UsersFolder, user.UserName, "events"));
            

            await _eventService.SaveEventAsync(newEvent);

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> GetEvents(int skip = 0,string? q="")
        {
            AppUser user = await _userManager.GetUserAsync(User);
            List<Event> events = await _eventService.GetEventsAsync(skip,q);
            List<EventCardVM> eventCards = new();
            EventCardVM eventCardVM = new();
            foreach (Event item in events)
            {
                eventCardVM = _mapper.Map<EventCardVM>(item);
                eventCardVM.IsIdenUserEvent =  item.AppUserId == user.Id;
                if (!eventCardVM.IsIdenUserEvent)
                {
                    eventCardVM.IsAttend = item.EventAttends.Any(et => et.AppUserId == user.Id);
                }
                eventCards.Add(eventCardVM);
            }
            return Json(eventCards);
        }



        public async Task<IActionResult> Attend(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Event AttendedEvent = await _eventService.GetEventAsync((int)id);
            if (AttendedEvent is null)
            {
                _notf.AddErrorToastMessage("Event not found");
                return RedirectToAction("Index");
            }
            AppUser user = await _userManager.GetUserAsync(User);
            if (AttendedEvent.EventAttends.Any(e => e.AppUserId == user.Id) || AttendedEvent.AppUserId == user.Id)
                return BadRequest();

            AttendedEvent.EventAttends.Add(new EventAttend
            {
                AppUser = user,
                Event = AttendedEvent
            });

            await _eventService.UpdateAttend(AttendedEvent);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> RemoveAttend(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Event AttendedEvent = await _eventService.GetEventAsync((int)id);
            if (AttendedEvent is null)
            {
                _notf.AddErrorToastMessage("Event not found");
                return RedirectToAction("Index");
            }
            AppUser user = await _userManager.GetUserAsync(User);
            EventAttend eventAttend =  AttendedEvent.EventAttends.FirstOrDefault(e => e.AppUserId == user.Id);
            if (eventAttend is null) return NotFound();
            bool result = await _eventService.RemoveAttend(eventAttend);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Event DeletedEvent = await _eventService.GetEventAsync((int)id);
            if (DeletedEvent is null)
            {
                _notf.AddErrorToastMessage("Event not found");
                return RedirectToAction("Index");
            }
            AppUser user = await _userManager.GetUserAsync(User);
            if (DeletedEvent.AppUserId != user.Id)
            {
                _notf.AddErrorToastMessage("You are not owner this event");
                return RedirectToAction("Index");
            }
            await _eventService.DeleteEvent(DeletedEvent);
            _notf.AddSuccessToastMessage("Succesfully deleted event");
            return RedirectToAction("Index");
        }
    }
}
