using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Zust.Application.Abstractions.Services;
using Zust.Application.Extensions;
using Zust.Application.ViewModels;
using Zust.Domain.Entities;
using Zust.Persistence.Contexts;

namespace Zust.Web.Controllers
{
    [Authorize]
   public class ProfilesController : Controller
    {
        IUserService _userService;
        IFriendService _friendService;
        IReactionService _reactionService;
        UserManager<AppUser> _userManager;
        IMapper _mapper;
        IToastNotification _notf;
        IPathService _path;
        AppDbContext _context;

        public ProfilesController(IUserService userService, UserManager<AppUser> userManager, IMapper mapper, IToastNotification notf, IPathService path, AppDbContext context, IFriendService friendService, IReactionService reactionService)
        {
            _userService = userService;
            _userManager = userManager;
            _mapper = mapper;
            _notf = notf;
            _path = path;
            _context = context;
            _friendService = friendService;
            _reactionService = reactionService;
        }



        public async Task<IActionResult> Index(string? username)
        {
           
            if (username is null) return BadRequest();
            AppUser FindedUser = await _userService.GetIdentityUserInfo(username);
            if (FindedUser is null || FindedUser.IsDeleted) return NotFound();
            AppUser IdenUser = await _userManager.GetUserAsync(User);
            ProfileInformationVM userVM = _mapper.Map<ProfileInformationVM>(FindedUser);
            userVM.FriendsCount = _friendService.GetFriendsList(FindedUser.Id).Count();
            userVM.LikeCount = await _reactionService.GetReactionsCount(FindedUser.Id);
            ProfileVM profileVM = new()
            {
                profileInformationVM = userVM
            };

            if (username != User.Identity.Name)
            {
                bool isFriend = await _friendService.IsFriend(IdenUser.Id,FindedUser.Id);
                bool? checkFriend = await _friendService.CheckFriend(IdenUser.Id, FindedUser.Id);
                profileVM.friendCheckVM = new FriendCheckVM()
                {
                    IsFriend = isFriend,
                    CheckFriend = checkFriend
                };

                if(FindedUser.PrivacySetting.WhoCanSeeYourProfile is null)
                    return NotFound();
                if (FindedUser.PrivacySetting.WhoCanSeeYourProfile == false)
                    if (isFriend)
                        return View(nameof(ProfilView), profileVM);
                    else
                        return NotFound();
                return View(nameof(ProfilView), profileVM);
            }

            return View(profileVM);
        }

        
        async Task<IActionResult> ProfilView()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Index(ProfileVM profil)
        {
            AppUser LoginedUser = await _userManager.GetUserAsync(User);
            
            if (profil.profileImageVM is not null)
            {
                var profilImageResult = SetProfilImage(profil.profileImageVM);
                if (profilImageResult.isValid)
                {
                    LoginedUser.ProfilImage.Delete(Path.Combine(_path.UsersFolder, User.Identity.Name));
                    LoginedUser.ProfilImage = profilImageResult.text;
                    await _userManager.UpdateAsync(LoginedUser);
                }
                else
                {
                    _notf.AddWarningToastMessage(profilImageResult.text);
                }
            }

            if (profil.bannerImageVM is not null)
            {
                var bannerImageResult = SetBannerImage(profil.bannerImageVM);
                if (bannerImageResult.isValid)
                {
                    LoginedUser.BannerImage.Delete(Path.Combine(_path.UsersFolder, User.Identity.Name));
                    LoginedUser.BannerImage = bannerImageResult.text;
                    await _userManager.UpdateAsync(LoginedUser);
                }
                else
                {
                    _notf.AddWarningToastMessage(bannerImageResult.text);
                }
            }


           
            var errors = ModelState?.FirstOrDefault(s => s.Key == "profileInformationVM.About").Value?.Errors;
            if (errors?.Count > 0) _notf.AddWarningToastMessage(errors?.FirstOrDefault()?.ErrorMessage ?? "Warning!");
            else
            {
                if(profil.profileInformationVM?.About is not null)
                {
                    LoginedUser.About = profil.profileInformationVM?.About;
                    await _userManager.UpdateAsync(LoginedUser);
                }

            }
            


            
            errors = ModelState?.FirstOrDefault(s => s.Key == "profileInformationVM.EducationOrWorks").Value?.Errors;
                
            if (errors?.Count > 0) _notf.AddWarningToastMessage(errors?.FirstOrDefault()?.ErrorMessage ?? "Warning!");
            else
            {
                if(profil.profileInformationVM?.EducationOrWorks is not null)
                {
                LoginedUser.EducationOrWorks = profil.profileInformationVM?.EducationOrWorks;
                await _userManager.UpdateAsync(LoginedUser);
                }
            }
            


            return RedirectToAction(nameof(Index), new { username = User.Identity.Name});
        }


       
        
        
       
        
        
        (string text,bool isValid) SetProfilImage(ProfileImageVM image)
        {
            IFormFile? profileImage = image?.ProfilImage;
            if (profileImage is not null)
            {
                string result = profileImage.CheckValidate(5, "image");
                if(result.Length > 0)
                    ModelState.AddModelError("ProfilImage", result);
            }

            var errors = ModelState?.FirstOrDefault(s => s.Key == "ProfilImage").Value?.Errors;
            if (errors?.Count > 0) return (errors?.FirstOrDefault()?.ErrorMessage??"",false);

            string filename = profileImage.SaveFile(Path.Combine(_path.UsersFolder, User.Identity?.Name ?? "other"));
            return (filename,true);
        }



        (string text,bool isValid) SetBannerImage(BannerImageVM banner)
        {
            IFormFile? bannerImage = banner?.Image;
            if (bannerImage is not null)
            {
                string result = bannerImage.CheckValidate(8, "image");
                if (result.Length > 0)
                    ModelState.AddModelError("BannerImage", result);
            }

            var errors = ModelState?.FirstOrDefault(s => s.Key == "BannerImage").Value?.Errors;
            if (errors?.Count > 0) return (errors?.FirstOrDefault()?.ErrorMessage ?? "Warning!", false);

            string filename = bannerImage.SaveFile(Path.Combine(_path.UsersFolder, User.Identity?.Name ?? "other"));
            return (filename, true);
        }

    
        
    }
}
