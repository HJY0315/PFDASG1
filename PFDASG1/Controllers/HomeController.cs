using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PFDASG1.DAL;
using PFDASG1.Models;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Xml.Linq;

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

        [HttpPost]
        public IActionResult AdjustFontSize(int size)
        {
            // Save the selected font size in the session or user preferences
            ControllerContext.HttpContext.Session.SetInt32("FontSize", size);

            // Redirect back to the referring page or any desired page
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public IActionResult UpdateFontSizeJS(int size)
        {
            // Save the selected font size in the session or user preferences
            ControllerContext.HttpContext.Session.SetInt32("FontSize", size);

            // Return an OK response or any necessary information
            return Ok();
        }

        [HttpGet]
        public IActionResult GetFontSize()
        {
            var fontSize = ControllerContext.HttpContext.Session.GetInt32("FontSize") ?? 20;
            return Json(new { fontSize = fontSize });
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
            string Pin = formData["txtPin"].ToString();
            string Name;
            int id;

            if (userContext.Login(Access_Code, Pin, out Name, out id))
            {
                HttpContext.Session.SetString("Name", Name);
                HttpContext.Session.SetInt32("id", id);
                

                return RedirectToAction("Index", "VisuallyImpaired");
            }
            else
            {
                TempData["Message"] = "Invalid login credentials.";
                ViewBag.ErrorMessage = "Invalid login credentials.";
                return View();
            }
        }





        public async Task<IActionResult> Logout()
        {
            // Sign out the user
            HttpContext.Session.Remove("Name");
            HttpContext.Session.Remove("id");
            // Redirect to the home page or any other page
            return RedirectToAction("Index", "Home");
        }
    }
}