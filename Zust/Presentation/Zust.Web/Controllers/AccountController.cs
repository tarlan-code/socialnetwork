using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Data;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Zust.Application.Abstractions.Services;
using Zust.Application.Extensions;
using Zust.Application.UnitOfWorks;
using Zust.Application.ViewModels;
using Zust.Domain.Entities;
using Zust.Domain.Enums;

namespace Zust.Web.Controllers
{
    public class AccountController : Controller
    {

        UserManager<AppUser> _userManager;
        RoleManager<IdentityRole> _roleManager;
        SignInManager<AppUser> _signInManager;
        IUnitOfWork _unitOfWork;
        IMapper _mapper;
        IMailService _mailService;
        IToastNotification _notf;
        IPathService _path;
        IPrivacySettingService _privacySettingService;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, IMapper mapper, IMailService mailService, IToastNotification notf, IPathService path, IPrivacySettingService privacySettingService, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _mailService = mailService;
            _notf = notf;
            _path = path;
            _privacySettingService = privacySettingService;
            _unitOfWork = unitOfWork;
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }



        #region Login
        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                    return RedirectToAction("Index", "Home", new { area = "Manage" });
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            
            if (User.Identity.IsAuthenticated) {
                if (User.IsInRole("Admin"))
                   return RedirectToAction("Index", "Home", new {area = "Manage"});

                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid) return View();

            AppUser user = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);
            if (user is null)
            {
                user = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
                if (user is null)
                {
                    ModelState.AddModelError("", "Login or password is wrong");
                    return View();
                }
            }

            
            
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);

            if (result.IsLockedOut && await _userManager.CheckPasswordAsync(user, loginVM.Password))
            {
                TimeSpan ts = (TimeSpan)(user.LockoutEnd - DateTime.UtcNow);
                ModelState.AddModelError("", $"Your account is locked due to an incorrectly entered password. Try again after {Math.Ceiling(ts.TotalMinutes)} minutes");
                return View();
            }

            
            if (!result.Succeeded)
            {
                if (!user.EmailConfirmed && await _userManager.CheckPasswordAsync(user,loginVM.Password))
                {
                    ModelState.AddModelError("", "Please Confirm Your Email. If you didn't receive the verification link, resend it.");
                    ViewBag.Confirmed = false;
                    ViewBag.Email = user.Email;
                    return View();
                }
                ModelState.AddModelError("", "Login or password is wrong");
                return View();
            }
            if (user.IsDeleted)
            {
                user.IsDeleted = false;
                user.DeletedDate = null;
                await _unitOfWork.SaveAsync();
            }            
            if (await _userManager.IsInRoleAsync(user,"Admin"))
                return RedirectToAction("Index", "Home", new { area = "Manage" });
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Registration
        public async Task<IActionResult> Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                if(User.IsInRole("Admin"))
                    return RedirectToAction("Index", "Home", new { area = "Manage" });
                return RedirectToAction("Index", "Home");
            }
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                    return RedirectToAction("Index", "Home", new { area = "Manage" });
                return RedirectToAction("Index", "Home");
            }
            string? CheckResult = registerVM.Birthday.CheckBirthday();
            if (CheckResult is not null)
                ModelState.AddModelError("Birthday", CheckResult);
            if (!ModelState.IsValid) return View();

            AppUser user = _mapper.Map<AppUser>(registerVM);
          

            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }


            result = await _userManager.AddToRoleAsync(user, Role.Name.Member.ToString());
            if (!result.Succeeded)
            {
                foreach (IdentityError item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }

            
            //Add Privacy Setting
            await _privacySettingService.AddPrivacySetting(new PrivacySetting { AppUser = user });


            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            string? url = Url.Action(nameof(Confirmemail), "Account", new { token, Email = user.Email }, Request.Scheme);

            string html = string.Empty;
            using(StreamReader sr = new StreamReader(Path.Combine(_path.HtmlFolder, "confirmmail.html"),encoding:Encoding.UTF8))
            {
                html = sr.ReadToEnd();
            }
            html = Regex.Replace(html,"confirmlink", url);

            await _mailService.SendMailAsync(user.Email, "Please Confirm Your Email", html);
            return RedirectToAction(nameof(SuccessEmailSend),"Account", new { text = "Successfully Registered Please Confirm Your Email"});
        }

        #endregion


        public async Task<IActionResult> Confirmemail(string token,string email)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user is null || token is null) return NotFound();
            IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded) BadRequest();
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction(nameof(SuccessEmailSend), new {text = "Successfully Confirmed Your Email" });
        }


        public async Task<IActionResult> SuccessEmailSend(string text)
        {
            ViewBag.Data = text;
            return View();
        }


        #region Sending Confirmation email
        public async Task<IActionResult> SendConfEmail(string email)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            if(user is null) return NotFound();
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            string? url = Url.Action(nameof(Confirmemail), "Account", new { token, Email = user.Email }, Request.Scheme);

            string html = string.Empty;
            using (StreamReader sr = new StreamReader(Path.Combine(_path.HtmlFolder,"confirmmail.html"), encoding: Encoding.UTF8))
            {
                html = sr.ReadToEnd();
            }
            html = Regex.Replace(html, "confirmlink", url);

            await _mailService.SendMailAsync(user.Email, "Please Confirm Your Email", html);
            _notf.AddSuccessToastMessage(message: "Successfully send email please check your email(and spam box)", new ToastrOptions
            {
                Title = "Success"
            });
            return RedirectToAction("Login");
        }
        #endregion



        #region Password Reset
        public async Task<IActionResult> PasswordReset()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordReset(PasswordResetVM resetVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.FindByEmailAsync(resetVM.UsernameOrEmail);
            if(user is null)
            {
                user = await _userManager.FindByNameAsync(resetVM.UsernameOrEmail);
                if(user is null)
                {
                    ModelState.AddModelError("UsernameOrEmail", "User not found");
                    return View();
                }
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string? url = Url.Action(nameof(UpdatePassword), "Account", new { token, email = user.Email }, Request.Scheme);
            string html = string.Empty;
            using (StreamReader sr = new StreamReader(Path.Combine(_path.HtmlFolder, "resetpassword.html"), encoding: Encoding.UTF8))
            {
                html = sr.ReadToEnd();
            }
            html = Regex.Replace(html, "action_url", url);
            html = Regex.Replace(html, "system", Request.Headers["User-Agent"].ToString());
            html = Regex.Replace(html, "username",user.UserName);
            await _mailService.SendMailAsync(user.Email, "Reset Passwor Zust Account", html);
            return RedirectToAction(nameof(SuccessEmailSend), new {text = "Succesfully send reset password mail" });
        }
        #endregion

        #region Update Password For Reset Password
        public async Task<IActionResult> UpdatePassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordVM updatePassword,string token, string email)
        {
            if (token is null || email is null) return BadRequest();
            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user is null) return NotFound();

            IdentityResult result = await _userManager.ResetPasswordAsync(user, token, updatePassword.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }

            return RedirectToAction(nameof(Login));
        }
        #endregion



        #region AddRoles
        public async Task<IActionResult> AddRoles()
        {
            foreach (var item in Enum.GetValues(typeof(Role.Name)))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
            }
            return Content("Ok");
        }
        #endregion


        #region AddOtherData
        public async Task<IActionResult> AddReactions()
        {
            //List<string> reactions = new List<string>() { "Like", "Heart", "Grinning", "Astonished", "Crying", "Enraged" };
            //foreach (var item in reactions)
            //{
            //    await _unitOfWork.GetWriteRepository<Reaction>().AddAsync(new Reaction { IconName = item });
            //}
            List<string> genders = new List<string>() { "Male", "Female" };
            List<string> relations = new List<string>() { "Maried", "Single" };
            foreach (var item in genders)
            {
                await _unitOfWork.GetWriteRepository<Gender>().AddAsync(new Gender { Name = item });
            }
            foreach (var item in relations)
            {
                await _unitOfWork.GetWriteRepository<Relation>().AddAsync(new Relation { Name = item });
            }
            await _unitOfWork.SaveAsync();
                return Ok();
        }
        #endregion




        #region Google Login and register

        public IActionResult GoogleLogin(string ReturnUrl)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            string? redirectUrl = Url.Action("ExternalResponse", "Account", new { ReturnUrl = ReturnUrl });
            AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);


        }



        public async Task<IActionResult> ExternalResponse(string ReturnUrl = "/")
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            ExternalLoginInfo loginInfo = await _signInManager.GetExternalLoginInfoAsync();

            if (loginInfo == null)
                return RedirectToAction("Login");
            else
            {
                Microsoft.AspNetCore.Identity.SignInResult loginResult = await _signInManager.ExternalLoginSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, true);
                if (loginResult.Succeeded)
                {
                    AppUser user = await _userManager.FindByEmailAsync(loginInfo.Principal.FindFirst(ClaimTypes.Email).Value);
                    if (user.IsDeleted)
                    {
                        user.IsDeleted = false;
                        await _unitOfWork.SaveAsync();
                    }
                    return Redirect(ReturnUrl);
                }
                else
                {
                    AppUser HasUser = await _userManager.FindByEmailAsync(loginInfo.Principal.FindFirst(ClaimTypes.Email).Value);
                    if(HasUser is not null)
                    {
                        _notf.AddInfoToastMessage("Please login with password", new ToastrOptions
                        {
                            Title = "There is an account in this mail"
                        });
                        return RedirectToAction(nameof(Login));
                    }


                 
                    AppUser user = new AppUser
                    {
                        Name = loginInfo.Principal.FindFirst(ClaimTypes.GivenName).Value,
                        Surname = loginInfo.Principal.FindFirst(ClaimTypes.Surname)?.Value,
                        Email = loginInfo.Principal.FindFirst(ClaimTypes.Email).Value,
                        UserName = loginInfo.Principal.FindFirst(ClaimTypes.Email).Value.Substring(0,loginInfo.Principal.FindFirst(ClaimTypes.Email).Value.LastIndexOf("@")),
                        Birthday = DateTime.Now.AddYears(-13),
                        EmailConfirmed = true,
                    };

                    IdentityResult createResult = await _userManager.CreateAsync(user);

                   
                    if (createResult.Succeeded)
                    {
                        IdentityResult resultAddRole = await _userManager.AddToRoleAsync(user, Role.Name.Member.ToString());
                        if (!resultAddRole.Succeeded)
                        {
                            foreach (IdentityError item in resultAddRole.Errors)
                            {
                                ModelState.AddModelError("", item.Description);
                            }
                            return View();
                        }
                        //AddPrivacySetting
                        await _privacySettingService.AddPrivacySetting(new PrivacySetting { AppUser = user });

                        IdentityResult addLoginResult = await _userManager.AddLoginAsync(user, loginInfo);
                        if (addLoginResult.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, true);
                            return Redirect(ReturnUrl);
                        }
                    }

                    

                }
            }
            return Redirect(ReturnUrl);
        }

        #endregion


    }
}
