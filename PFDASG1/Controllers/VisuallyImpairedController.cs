using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PFDASG1.DAL;
using PFDASG1.Models;
using System.Diagnostics.Metrics;
using System.Transactions;

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
            ViewData["CardActivation"] = null;
            return View();
        }

        [HttpPost]
        public IActionResult CardActivation(CreditCard cardinfo, string selectedMonth)
        {
            // Access the form data through the model properties
            var cardNumber1 = cardinfo.cardNumber1;
            var cardNumber2 = cardinfo.cardNumber2;
            var cardHolder = cardinfo.cardHolderName;
            var expirationMonth = cardinfo.expirationMonth;
            var expirationYear = cardinfo.expirationYear;
            var ccv = cardinfo.cvv;
            var securityQuestionNo = cardinfo.securityQuestion;
            var securityAnswer = cardinfo.answer;
            var securityQuestion = "";

            if (securityQuestionNo == "1")
            {
                securityQuestion = "Where is your primary school?";
            }
            else if (securityQuestionNo == "2")
            {
                securityQuestion = "Who is your best friend?";
            }
            else if (securityQuestionNo == "3")
            {
                securityQuestion = "Do you have any musical background?";
            }


            string msg = "";
            int cardID = CardActivationContext.cardVerification(cardinfo,out msg, securityQuestion);
            if (msg == "")
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


        [HttpGet]
        public IActionResult GetCurrentBalance()
        {
            // Get the user's ID from the session
            int sessionId = HttpContext.Session.GetInt32("id") ?? 0;

            // Retrieve all transactions for the specified user
            List<Transactions> transactions = TransactionsContext.getalltransactions(sessionId);

            // Initialize the total balance variable
            decimal totalBalance = 0;

            // Add amounts for non-sender transactions
            foreach (var item in transactions)
            {
                if (item.SenderID != sessionId)
                {
                    totalBalance += item.Amount;
                }
            }

            // Subtract amounts for sender transactions
            var senderTransactions = transactions.Where(item => item.SenderID == sessionId);
            decimal amountToSubtract = senderTransactions.Sum(item => item.Amount);
            totalBalance -= amountToSubtract;

            // Return the total balance
            return Json(new { totalBalance });
        }



        public IActionResult Index(int userId, decimal amount)
        {
            // Get the user's name from the session
            string userName = HttpContext.Session.GetString("Name");
            ViewBag.UserName = userName;

            // Get the user's ID from the session
            int sessionId = HttpContext.Session.GetInt32("id") ?? 0;

            // Retrieve transactions based on the session ID
            List<Transactions> transactions = TransactionsContext.getalltransactions(sessionId);
            // Initialize the total balance variable
            decimal totalBalance = 0;

            // Add amounts for non-sender transactions
            foreach (var item in transactions)
            {
                if (item.SenderID != sessionId)
                {
                    totalBalance += item.Amount;
                }
                else
                {
                    totalBalance -= item.Amount;
                }
            }

            // Subtract amounts for sender transactions
            var senderTransactions = transactions.Where(item => item.SenderID == sessionId);
            decimal amountToSubtract = senderTransactions.Sum(item => item.Amount);
            totalBalance -= amountToSubtract;
            decimal despostamount = totalBalance - amountToSubtract;
            decimal withdrawlamount = despostamount - amountToSubtract;

            ViewBag.amountToSubtract = amountToSubtract;
            ViewBag.totalBalance = despostamount;


            return View(transactions);
        }

        public IActionResult Search()
        {
            List<Pages> pagesList = SearchDAL.GetAllPages();
            return View(pagesList);
        }

    }
}