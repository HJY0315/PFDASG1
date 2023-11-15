using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PFDASG1.DAL;
using PFDASG1.Models;

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

        public IActionResult Index(string userName, int id)
        {
            // Use the userName parameter as needed in this action
            ViewBag.UserName = userName;
            
            List<Transactions> transactions = TransactionsContext.getalltransactions(id);
            return View(transactions);
        }
  

        public IActionResult Search()
        {
            List<Pages> pagesList = SearchDAL.GetAllPages();
            return View(pagesList);
        }

    }
}
