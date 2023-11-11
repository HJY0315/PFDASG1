using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PFDASG1.Controllers
{
    public class VisuallyImpairedController : Controller
    {

        public IActionResult Settings()
        {
            return View();
        }
    

        public IActionResult Index(string userName)
        {
            // Use the userName parameter as needed in this action
            ViewBag.UserName = userName;

            return View();
        }


        public IActionResult Search()
        {
            return View();
        }

    }
}
