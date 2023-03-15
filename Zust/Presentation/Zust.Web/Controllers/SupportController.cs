using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Zust.Application.Abstractions.Services;
using Zust.Application.ViewModels;
using Zust.Domain.Entities;

namespace Zust.Web.Controllers
{
    [Authorize]
    [Authorize(Roles = "Member")]

    public class SupportController : Controller
    {
        ISupportService _supportService;
        IMapper _mapper;
        IToastNotification _notf;
        UserManager<AppUser> _userManager;

        public SupportController(ISupportService supportService, IMapper mapper, UserManager<AppUser> userManager, IToastNotification notf)
        {
            _supportService = supportService;
            _mapper = mapper;
            _userManager = userManager;
            _notf = notf;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(SupportContactVM contact)
        {
            if(!ModelState.IsValid) return View(contact);

            Contact newContact = new();
            newContact =  _mapper.Map<Contact>(contact);
            newContact.AppUser = await _userManager.GetUserAsync(User);
            await _supportService.AddContact(newContact);
            _notf.AddSuccessToastMessage("Succesfully send contact message");
            return RedirectToAction("Index");
        } 
    }
}
