﻿using Microsoft.AspNetCore.Mvc;
using PFDASG1.DAL;
using PFDASG1.Models;


namespace PFDASG1.Controllers
{
    public class TransactionsController : Controller
    {
        //TransactionsDAL TransactionsContext = new TransactionsDAL();
        //public TransactionsController()
        //{
        //    return;
        //}

        //public IActionResult Index()
        //{
        //    int userId = (int)HttpContext.Session.GetInt32("Userid");
        //    List<Transactions> transactions = TransactionsContext.getalltransactions(userId);
        //    return View(transactions);

        //}
        //[HttpGet]
        //public IActionResult Transfer() {
        //    return View();

        //}
        

    
        //[HttpPost]
        //public ActionResult Transfer(Transactions transactions)
        //{
        //    try
        //    {
        //        int userId = (int)HttpContext.Session.GetInt32("Userid");
        //        Transactions transaction = new Transactions();
        //        User user

        //        TransactionsContext.Add(transactions, userId, transactions.phoneNumber);






                //TempData["message"] = "Money has successfully been transferred";
                //TempData["status"] = "success";
                //return View("Transfer");

        //        TempData["message"] = $"Failed to Transfer. Error: {ex.Message}";
        //        TempData["status"] = "danger";
        //        return RedirectToAction("Transfer");
        //    }

        //}
    }
}
