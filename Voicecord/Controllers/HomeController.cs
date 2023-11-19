using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Voicecord.Models;

namespace Voicecord.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.user = "fefe";
            return View();
        }

        [HttpPost]
        public string CreateGroup(string name)
        {


            //db.UserGroups.Add(new UserGroup()
            //{
            //    Name = name,
            //    LinkImageGroup = "somelink",
            //    Users = new List<ApplicationUser>() {}
            //});
            return string.Empty;
        }

        public IActionResult CreateGroup()
        {
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
}