using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zust.Application.Abstractions.Services;
using Zust.Application.UnitOfWorks;
using Zust.Application.ViewModels;
using Zust.Domain.Entities;

namespace Zust.Web.Controllers
{

    [Authorize]
    [Authorize(Roles = "Member")]
    public class HomeController : Controller
    {

        IUserService _userService;
        IFriendService _friendService;
        IBirthdayService _birthdayService;
        IMapper _mapper;
        IUnitOfWork _unit;
        UserManager<AppUser> _userManager;
        public HomeController(IGenderService genderService, IUserService userService, IMapper mapper, IFriendService friendService, UserManager<AppUser> userManager, IUnitOfWork unit, IBirthdayService birthdayService)
        {
            _userService = userService;
            _mapper = mapper;
            _friendService = friendService;
            _userManager = userManager;
            _unit = unit;
            _birthdayService = birthdayService;
        }

        public async Task<IActionResult> Index()
        {

            return View();
        }


        #region AddCountries and Cities
        //public class CountryJs
        //{
        //    public string name { get; set; }
        //    public List<CityJS> states { get; set; }
        //}


        //public class CityJS
        //{
        //    public string name { get; set; }
        //}


      
        //public async Task<IActionResult> Add()
        //{

        //    ServicePointManager.Expect100Continue = true;
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //    string json = (new WebClient()).DownloadString("https://raw.githubusercontent.com/dr5hn/countries-states-cities-database/master/countries%2Bstates.json");
        //    var liste = (List<CountryJs>)JsonConvert.DeserializeObject(json, typeof(List<CountryJs>));

        //    foreach (var item in liste.ToList())
        //    {
        //        //await _unit.GetWriteRepository<Country>().AddAsync(new Country { Name = item.name.ToString()});
        //        Country country = await _unit.GetReadRepository<Country>().GetAsync(x => x.Name == item.name);
        //        foreach (var city in item.states.ToList())
        //        {
        //            await _unit.GetWriteRepository<City>().AddAsync(new City { Name = city.name.ToString(), CountryId = country.Id });
        //        }
        //    }
        //    await _unit.SaveAsync();
        //    return Ok();
        //}
        #endregion


        [HttpPost]
        public async Task<IActionResult> GetSearchedUsers(string text)
        {
            List<AppUser> users = _userService.GetSearchedUsers(User.Identity.Name, text);
            List<SearchedUserVM> searchedUsers = new();
            AppUser IdenUser = await _userManager.GetUserAsync(User);
            foreach (var user in users)
            {
                if(user.PrivacySetting.WhoCanSeeYourProfile != null)
                {
                    if (user.PrivacySetting.WhoCanSeeYourProfile==false)
                    {
                        if(await _friendService.IsFriend(user.Id,IdenUser.Id))
                        {
                            searchedUsers.Add(_mapper.Map<SearchedUserVM>(user));
                        }
                    }
                    else
                    {
                        searchedUsers.Add(_mapper.Map<SearchedUserVM>(user));
                    }
                }
            }
            return Json(searchedUsers);
        }

    }
}