using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PFDASG1.DAL;
using PFDASG1.Models;

namespace PFDASG1.Controllers
{
    public class VisuallyImpairedController : Controller
    {
        private SearchDAL SearchDAL = new SearchDAL();

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
            List<Pages> pagesList = SearchDAL.GetAllPages();
            return View(pagesList);
        }

    }
}
