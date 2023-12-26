﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PFDASG1.DAL;
using PFDASG1.Models;
using System.Diagnostics.Metrics;
using System.Dynamic;
using System.Transactions;
using System.Xml.Linq;

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
            if (HttpContext.Session.GetString("Name") != null)
            {
                TempData["ShowSQSetUp"] = false; //if user is logged in, then dn setup
            }
            if (TempData["showSQSetUp"] == null)
            {
                return RedirectToAction("SQSetUp");
            }

            return View();
        }

        [HttpPost]
        public IActionResult CardActivation(CreditCard cardinfo, SecurityQuestion questionAnswer = null)
        {
            ViewBag.UserName = HttpContext.Session.GetString("Name");
            if (HttpContext.Session.GetString("Name") != null)
            {
                TempData["ShowSQSetUp"] = false; //if user is logged in, then dn setup
            }

            var securityQuestionNo = cardinfo.securityQuestion;
            var inputSecurityQuestion = "";


            if (securityQuestionNo == "1")
            {
                inputSecurityQuestion = "Where is your primary school?";
            }
            else if (securityQuestionNo == "2")
            {
                inputSecurityQuestion = "Who is your best friend?";
            }
            else if (securityQuestionNo == "3")
            {
                inputSecurityQuestion = "Do you have any musical background?";
            }

            var sq = HttpContext.Session.GetString("SQ");
            var answer = HttpContext.Session.GetString("answer");

            string msg = "";
            if ((sq != null) && (answer != null)) //have temp question and answer
            {
                if ((sq == cardinfo.securityQuestion))
                {
                    if (answer.ToString() == cardinfo.answer.ToString())
                    {
                        //i leave it empty on purpose
                    }
                    else
                    {
                        msg = "Verification failed.";
                    }
                }
                else
                {
                    msg = "Verification failed.";
                }
            }
            if (HttpContext.Session.GetString("Name") != null)
            {
                var loggedinUserName = HttpContext.Session.GetString("Name");
                var loggedinUserID = HttpContext.Session.GetInt32("id");

                string userIdAsString = loggedinUserID.Value.ToString();
                CardActivationContext.VerifySecurityQuestionAnswer(userIdAsString, cardinfo, inputSecurityQuestion, out msg);
            }
            if (msg != "")
            {
                ViewData["CardActivation"] = msg;
            }
            else //if they pass verification
            {
                int cardID = CardActivationContext.cardVerification(cardinfo, out msg);
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
            }
            
            return View();
        }

        [HttpGet]
        public IActionResult Transfer()
        {
            TransactionViewModel transactionViewModel = new TransactionViewModel();

            return View(transactionViewModel);
        }

        [HttpGet]
        public IActionResult SearchPhoneNumber(string phoneNumber)
        {
            List<User> userList = TransactionsContext.GetUsersByPhoneNumber(phoneNumber);

            return View(userList);
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
            decimal despostamount = totalBalance;
            decimal withdrawlamount = totalBalance - amountToSubtract;

            ViewBag.amountToSubtract = amountToSubtract;
            ViewBag.totalBalance = despostamount;


            return View(transactions);
        }

        public IActionResult Search()
        {
            List<Pages> pagesList = SearchDAL.GetAllPages();
            return View(pagesList);
        }

        public IActionResult SQSetUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SQSetUp(SecurityQuestion questionAnswer)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                TempData["ShowSQSetUp"] = false; //if user is logged in, then dn setup
            }
            HttpContext.Session.SetString("SQ", questionAnswer.SQ);
            HttpContext.Session.SetString("answer", questionAnswer.answer);
            TempData["showSQSetUp"] = false;
            return RedirectToAction("CardActivation", new { questionAnswer = questionAnswer });
        }

    }
}