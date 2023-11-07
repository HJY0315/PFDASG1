using Microsoft.AspNetCore.Mvc;
using PFDASG1.Models;
using System.Diagnostics;

namespace PFDASG1.Controllers
{
    public class HomeController : Controller
    {
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
        //private MemberDAL memberContext = new MemberDAL();
        //[HttpPost]
        //public ActionResult MemberLogin(IFormCollection formData)
        //{
        //    // Read inputs from textboxes
        //    // Email address converted to lowercase
        //    string loginID = formData["txtLoginID"].ToString().ToLower();

        //    string password = formData["txtPassword"].ToString();

        //    string memberName;

        //    int memberid;

        //    if (memberContext.Login(loginID, password, out memberName, out memberid))
        //    {
        //        // Store Login ID in session with the key "LoginID"
        //        HttpContext.Session.SetString("LoginID", loginID);

        //        HttpContext.Session.SetString("Role", "Member");

        //        HttpContext.Session.SetString("Name", memberName);

        //        HttpContext.Session.SetInt32("MemberID", memberid);

        //        HttpContext.Session.SetString("LoggedInTime", DateTime.Now.ToString());

        //        // Redirect user to the "StaffMain" view through an action
        //        return RedirectToAction("MemberMain");
        //    }

        //    else
        //    {
        //        // Store an error message in TempData for display at the index view
        //        TempData["Message"] = "Invalid Login Credentials!";

        //        // Redirect user back to the index view through an action
        //        return RedirectToAction("Index");
        //    }
        //}
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