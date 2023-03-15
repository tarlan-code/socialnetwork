using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Zust.Application.Abstractions.Services;
using Zust.Application.ViewModels;
using Zust.Domain.Entities;

namespace Zust.Web.Areas.Manage.Controllers
{
    [Area(nameof(Manage))]
    [Authorize(Roles = "Admin")]

    public class ContactsController : Controller
    {
        ISupportService _supportService;
        IMapper _mapper;
        IToastNotification _notf;

        public ContactsController(ISupportService supportService, IMapper mapper, IToastNotification notf)
        {
            _supportService = supportService;
            _mapper = mapper;
            _notf = notf;
        }

        public async Task<IActionResult> Index()
        {
            List<Contact> contacts = await _supportService.GetContacts();
            List<PanelContactVM> panelContacts = new();
            contacts.ForEach(c => panelContacts.Add(_mapper.Map<PanelContactVM>(c)));
            return View(panelContacts);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id is null || id<=0) return BadRequest();
            Contact contact = await _supportService.GetContactAsync((int)id);
            if(contact is null) return BadRequest();
            await _supportService.DeleteContact(contact);
            _notf.AddSuccessToastMessage("Succesfully delete contact");
            return RedirectToAction(nameof(Index));
        }
    }
}
