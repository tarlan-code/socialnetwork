using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using System.Text;
using System.Text.RegularExpressions;
using Zust.Application.Abstractions.Services;
using Zust.Application.Extensions;
using Zust.Application.ViewModels;
using Zust.Domain.Entities;

namespace Zust.Web.Controllers
{
    [Authorize]
    [Authorize(Roles = "Member")]

    public class SettingsController : Controller
    {

        ICountryService _countryService;
        ICityService _cityService;
        IUserService _userService;
        IGenderService _genderService;
        IRelationService _relationService;
        IPrivacySettingService _privacySettingService;
        IMapper _mapper;
        IToastNotification _notf;
        IMailService _mailService;
        IPathService _path;
        UserManager<AppUser> _userManager;
        SignInManager<AppUser> _signInManager;

        public SettingsController(ICountryService countryService, IMapper mapper, ICityService cityService, IGenderService genderService, IRelationService relationService, IToastNotification notf, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMailService mailService, IPathService path, IPrivacySettingService privacySettingService, IUserService userService)
        {
            _countryService = countryService;
            _mapper = mapper;
            _cityService = cityService;
            _genderService = genderService;
            _relationService = relationService;
            _notf = notf;
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
            _path = path;
            _privacySettingService = privacySettingService;
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            SettingInfoVM settingInfo = _mapper.Map<SettingInfoVM>(user);
            ViewBag.Country = new SelectList(_countryService.GetAllCountry(), nameof(Country.Id), nameof(Country.Name));
            ViewBag.Cities = new SelectList(_cityService.GetCities(user.CountryId ?? 0), nameof(Country.Id), nameof(Country.Name));
            ViewBag.Genders = new SelectList(_genderService.GetAllGender(), nameof(Gender.Id), nameof(Gender.Name));
            ViewBag.Relations = new SelectList(_relationService.GetAllRelations(),nameof(Relation.Id),nameof(Relation.Name));
            return View(settingInfo);
            
        }

        [HttpPost]
        public async Task<IActionResult> Index(SettingInfoVM setting)
        {
            if (setting is null) BadRequest();

            

            if(setting.CountryId is not null && !await _countryService.CountryAnyAsync((int)setting.CountryId))
                ModelState.AddModelError("CountryId", "Country is wrong");
            

            if (setting.CityId is not null && setting.CountryId is null && !await _cityService.CityAnyAsync((int)setting.CityId, (int)setting.CountryId))
                ModelState.AddModelError("CityId", "City is wrong");

            if (setting.GenderId is not null && !await _genderService.GenderAnyAsync((int)setting.GenderId))
                ModelState.AddModelError("GenderId", "Gender is wrong");

            if (setting.RelationId is not null && !await _relationService.RelationAnyAsync((int)setting.RelationId))
                ModelState.AddModelError("RelationId", "Relation is wrong");

            string? resultBirthday = setting.Birthday.CheckBirthday();
            if (resultBirthday is not null)
                ModelState.AddModelError("Birthday", resultBirthday);

            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (!ModelState.IsValid)
            {
                ViewBag.Country = new SelectList(_countryService.GetAllCountry(), nameof(Country.Id), nameof(Country.Name));
                ViewBag.Cities = new SelectList(_cityService.GetCities(user.CountryId ?? 0), nameof(Country.Id), nameof(Country.Name));
                ViewBag.Genders = new SelectList(_genderService.GetAllGender(), nameof(Gender.Id), nameof(Gender.Name));
                ViewBag.Relations = new SelectList(_relationService.GetAllRelations(), nameof(Relation.Id), nameof(Relation.Name));
                return View(setting);
            }


            
            
            _mapper.Map(setting, user);
            IdentityResult result =  await _userManager.UpdateAsync(user);


            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    _notf.AddErrorToastMessage(item.Description, new ToastrOptions
                    {
                        Title = item.Code
                    });
                }
                return RedirectToAction(nameof(Index));
            }
            _path.UsersFolder.ChangeDirectoryName(User.Identity.Name, setting.Username);
            await UpdateEmail(user, setting.Email,user.Email);
            await _signInManager.RefreshSignInAsync(user);
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> GetCities(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            List<City> cities = _cityService.GetCities((int)id);
            if (cities is null) return NotFound();
            return Json(cities);
        }


        async Task UpdateEmail(AppUser user,string newEmail,string oldEmail)
        {
            if(newEmail != oldEmail)
            {
                string token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
                string? url = Url.Action(nameof(Confirmemail), "Settings", new { token, email = newEmail,oldemail=oldEmail}, Request.Scheme);
                string html = string.Empty;
                using (StreamReader sr = new StreamReader(Path.Combine(_path.HtmlFolder, "confirmmail.html"), encoding: Encoding.UTF8))
                {
                    html = sr.ReadToEnd();
                }
                html = Regex.Replace(html, "confirmlink", url);
                await _mailService.SendMailAsync(newEmail, "Confirm Your New Email",html);
                _notf.AddSuccessToastMessage("Succefully send confirmation link to new e-mail please check your email(spam)");
            }
        }

        public async Task<IActionResult> Confirmemail(string token,string email,string oldemail)
        {
            if (token is null || email is null) return BadRequest();
            AppUser user = await _userManager.FindByEmailAsync(oldemail);
            if (user is null) return NotFound();

            IdentityResult result = await _userManager.ChangeEmailAsync(user,email,token);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    _notf.AddErrorToastMessage(item.Description, new ToastrOptions
                    {
                        Title = item.Description
                    });
                }
                return RedirectToAction(nameof(Index));
            }
            _notf.AddSuccessToastMessage("Succesfully email confirmed");

            return RedirectToAction(nameof(Index));
        }





        public async Task<IActionResult> SetPrivcySetting(PrivacyVM privacy)
        {
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState)
                {
                    if(item.Value.Errors.Count > 0)
                        _notf.AddErrorToastMessage(item.Value.Errors.FirstOrDefault().ErrorMessage,new ToastrOptions
                        {
                            Title = item.Key
                        });
                }
            }
            AppUser user = await _userManager.GetUserAsync(User);
            PrivacySetting oldSetting =  await _privacySettingService.GetPrivacySettingAsync(user.Id);
            _mapper.Map(privacy, oldSetting);
            bool result =  _privacySettingService.UpdatePrivacySetting(oldSetting);
            if (!result)
                _notf.AddErrorToastMessage("Something is wrong", new ToastrOptions { Title = "System error" });
            
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> DeleteAccount(DeleteAccountVM deleteAccount)
        {
            if (deleteAccount is null) return BadRequest();
            
            AppUser user = await _userManager.GetUserAsync(User);

            if (user is null || user.Email != deleteAccount.Email)
                ModelState.AddModelError("", "UserNotFound");
            if(user is not null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, deleteAccount.Password,true,true);
                if (!result.Succeeded)
                ModelState.AddModelError("", "Email or password is wrong");
            }

            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState)
                {
                    if (item.Value.Errors.Count > 0)
                        _notf.AddErrorToastMessage(item.Value.Errors.FirstOrDefault().ErrorMessage, new ToastrOptions
                        {
                            Title = item.Key
                        });
                }
                return RedirectToAction(nameof(Index));
            }

            user.IsDeleted = true;
            user.DeletedDate = DateTime.Now.AddDays(30);

            await _userManager.UpdateAsync(user);
            await _signInManager.SignOutAsync();
            _notf.AddSuccessToastMessage("Account Deleted");
            return RedirectToAction(nameof(Index));

        }
    }
}
