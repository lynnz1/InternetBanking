using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InternetBanking.Data;
using InternetBanking.Models;
using InternetBanking.Filters;
using InternetBanking;
using Microsoft.EntityFrameworkCore;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InternetBanking.Controllers
{
    [AuthorizeCustomer]
    public class MyStatementController : Controller
    {
        private readonly MyContext _context;

        // ReSharper disable once PossibleInvalidOperationException
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public MyStatementController(MyContext context) => _context = context;


        public async Task<IActionResult> Index()
        {
            var customer = await _context.Customer.FindAsync(CustomerID);
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Index(int AccountNumber)
        {

            var account = await _context.Account.FindAsync(AccountNumber);

            HttpContext.Session.SetInt32("CurrentAccount", account.AccountNumber);

            return RedirectToAction("Statements", new { id = 0 });
        }

        [Route("/MyStatement/Statements/{id}")]
        public async Task<IActionResult> Statements(int id)
        {
            
            var accountNumber = HttpContext.Session.GetInt32("CurrentAccount");
            var account = await _context.Account.FindAsync(accountNumber);
            // when id = 0, retrieve skip 0(0*4) rows and get the next 5 rows.
            // if the fifth row is empty, it means the user have reach to the last row of DB.
            var t = account.Transactions.OrderByDescending(t => t.TransactionTimeUtc).Skip(id * 4).Take(5);
            StatementViewModel statement = new()
            {
                Transactions = new List<Transaction>(t),
                CurrentPage = id,
                Balance = account.Balance,
                AccountNumber = account.AccountNumber,
                LastPage = false
            };
            if (statement.Transactions.Count < 5)
            {
                statement.LastPage = true;
            }
            return View(statement);

        }

    }
}

