using Microsoft.AspNetCore.Mvc;

namespace PFDASG1.Controllers
{
    public class VisuallyImpairedController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
