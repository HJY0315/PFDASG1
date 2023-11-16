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
        CardActivationDAL CardActivationContext = new CardActivationDAL();
        User user;
        

        public IActionResult CardActivation()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CardActivation(CreditCard cardinfo)
        {
            // Access the form data through the model properties
            var cardNumber = cardinfo.cardNumber;
            var cardHolder = cardinfo.cardHolderName;
            var expirationDate = cardinfo.expirationDate;
            var ccv = cardinfo.cvv;
            var securityQuestion = cardinfo.securityQuestion;
            var securityAnswer = cardinfo.answer;

            string msg = "";
            int cardID = CardActivationContext.cardVerification(cardinfo,out msg);
            if (msg != "")
            {
                bool ActivationSuccess = CardActivationContext.UpdateCardStatus(cardID);
                if (ActivationSuccess)
                {
                    ViewData["CardActivation"] = "Card Activated Successfully";
                }
                else
                {
                    ViewData["CardActivation"] = "Card Activation Failed";
                }
            }
            else
            {
                ViewData["CardActivation"] = msg; //if the card is alr been activated
            }

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

        public IActionResult Unknown()
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