using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PFDASG1.DAL;
using PFDASG1.Models;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace PFDASG1.Controllers
{
    public class HomeController : Controller
    {
        private UserDAL userContext = new UserDAL();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
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
        [HttpPost]
        public ActionResult Login(IFormCollection formData)
        {
            string Access_Code = formData["txtLoginID"];
            string Pin = formData["txtPin"];
            

            if (userContext.Login(Access_Code, Pin) != null)
            {
                User user = userContext.Login(Access_Code, Pin);
                HttpContext.Session.SetString("Name", user.Name);
                HttpContext.Session.SetInt32("id", user.Userid);
                return RedirectToAction("Index", "VisuallyImpaired", new { userName = user.Name });
            }
            
             
            else
            {
                TempData["Message"] = "Invalid login credentials";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            // Sign out the user
            await HttpContext.SignOutAsync();

            // Redirect to the home page or any other page
            return RedirectToAction("Index", "Home");
        }
    }
}