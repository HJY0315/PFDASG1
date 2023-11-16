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

        


        

        [HttpGet]
        public IActionResult Transfer()
        {
            return View();
        }



        [HttpPost]
        public IActionResult Transfer(Transactions transaction)
        {
            ModelState.Remove("TransactionDate");
            ModelState.Remove("NRIC");
            ModelState.Remove("MobileNumber");
            ModelState.Remove("AccountId");

            int userId = (int)HttpContext.Session.GetInt32("id");
            if (ModelState.IsValid)
            {
                //transaction = new Transactions();
                TransactionsContext.Add(transaction, userId);
                TempData["message"] = "Money has successfully been transferred";
                TempData["status"] = "success";

                return RedirectToAction("Index"); // Redirect to a success page
            }
            else
            {

                return View(transaction);
            }
            // If the model state is not valid, return to the form with validation errors

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