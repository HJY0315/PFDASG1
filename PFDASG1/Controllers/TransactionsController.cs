using Microsoft.AspNetCore.Mvc;
using PFDASG1.DAL;
using PFDASG1.Models;


namespace PFDASG1.Controllers
{
    public class TransactionsController : Controller
    {
        TransactionsDAL TransactionsContext = new TransactionsDAL();
        public TransactionsController()
        {
            return;
        }

        public IActionResult Index()
        {
            int userId = (int)HttpContext.Session.GetInt32("Userid");
            List<Transactions> transactions = TransactionsContext.getalltransactions(userId);
            return View(transactions);

        }

    }
}
