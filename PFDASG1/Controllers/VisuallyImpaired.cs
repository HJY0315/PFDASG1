using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PFDASG1.Controllers
{
    public class VisuallyImpaired : Controller
    {
        // GET: VisuallyImpaired
        public IActionResult Login()
        {
            return View();
        }

        //public IActionResult Login()
        //{
        //    return View();
        //}
    }
}
