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

        


        

        [HttpGet]
        public IActionResult Transfer()
        {
            TransactionViewModel transactionViewModel = new TransactionViewModel();
            return View(transactionViewModel);
        }

        [HttpPost]
        public ActionResult Transfer(TransactionViewModel transactionViewModel)
        {
            int userId = (int)HttpContext.Session.GetInt32("id");

            //try
            //{
            // Retrieve the phone number from the form
            string phoneNumber = transactionViewModel.phoneNumber;
            User user = TransactionsContext.GetUserFromPhoneNumber(phoneNumber);

            transactionViewModel.receipient = user;
            transactionViewModel.TransactionDate = DateTime.Now;
            transactionViewModel.senderID = userId;
            transactionViewModel.AccountId = userId;
            // Create a new Transactions object

            // Add the transaction to the TransactionsContext
            TransactionsContext.Add(transactionViewModel);

            // Set the TempData variables to display a success message
            TempData["message"] = "Money has successfully been transferred";
            TempData["status"] = "success";
            //}
            //catch (Exception ex)
            //{
            //    // Handle any errors that occur
            //    TempData["message"] = "An error occurred during the transfer: " + ex.Message;
            //    TempData["status"] = "error";
            //}

            //// Redirect the user back to the Transfer page
            return RedirectToAction("Index");
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