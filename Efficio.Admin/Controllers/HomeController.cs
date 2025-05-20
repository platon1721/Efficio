// using System.Diagnostics;
// using Microsoft.AspNetCore.Mvc;
// using Efficio.Admin.Models;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Authorization;
//
// namespace Efficio.Admin.Controllers;
//
// public class HomeController : Controller
// {
//     private readonly ILogger<HomeController> _logger;
//     private readonly SignInManager<IdentityUser> _signInManager;
//
//     public HomeController(ILogger<HomeController> logger, SignInManager<IdentityUser> signInManager)
//     {
//         _logger = logger;
//         _signInManager = signInManager;
//     }
//
//     public IActionResult Index()
//     {
//         // Kui kasutaja on sisse logitud, suuna ta Admin alale
//         if (_signInManager.IsSignedIn(User))
//         {
//             return RedirectToAction("Index", "Home", new { area = "Admin" });
//         }
//         
//         // Vastasel juhul näita tavalist avaalehte
//         return View();
//     }
//
//     public IActionResult Privacy()
//     {
//         return View();
//     }
//
//     [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//     public IActionResult Error()
//     {
//         return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//     }
// }

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Efficio.Admin.Models;

namespace Efficio.Admin.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // Lihtne avaleht, mis sisaldab linki sisselogimiseks
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}