using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Zust.Web.Controllers
{
    [Authorize]
    [Authorize(Roles = "Member")]

    public class PrivacyController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
