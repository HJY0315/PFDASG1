using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PFDASG1.DAL;
using PFDASG1.Models;
using System.Diagnostics.Metrics;

namespace PFDASG1.Controllers
{
    public class VisuallyImpairedController : Controller
    {
        private SearchDAL SearchDAL = new SearchDAL();
        TransactionsDAL TransactionsContext = new TransactionsDAL();
        User user;
        

        public IActionResult Settings()
        {
            return View();
        }
    
        public IActionResult Transfer()
        {
            return View();
        }



        public IActionResult ViewAllCards()
        {
            return View();
        }

        public IActionResult Index()
        {
            // Use the userName parameter as needed in this action
            string Name = HttpContext.Session.GetString("Name");
            ViewBag.UserName = Name;
            int Id = HttpContext.Session.GetInt32("id") ?? 0;
            List<Transactions> transactions = TransactionsContext.getalltransactions(Id);
            return View(transactions);
        }
  

        public IActionResult Search()
        {
            List<Pages> pagesList = SearchDAL.GetAllPages();
            return View(pagesList);
        }

    }
}
