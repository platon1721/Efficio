using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Efficio.Admin.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : AdminBaseController
    {
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(IStringLocalizer<HomeController> localizer)
        {
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = _localizer["AdminDashboard"];
            return View();
        }
    }
}