using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using s3512958_a2.Data;
using s3512958_a2.Models;
using s3512958_a2.Filters;
using Microsoft.EntityFrameworkCore;
using SimpleHashing;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace s3512958_a2.Controllers
{
    [AuthorizeCustomer]
    public class CustomerController : Controller
    {
        private readonly MyContext _context;

        // ReSharper disable once PossibleInvalidOperationException
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public CustomerController(MyContext context) => _context = context;

        // Can add authorize attribute to actions.
        //[AuthorizeCustomer]
        public async Task<IActionResult> Index()
        {
            // Lazy loading.
            // The Customer.Accounts property will be lazy loaded upon demand.
            var customer = await _context.Customer.FindAsync(CustomerID);

            // OR
            // Eager loading.
            //var customer = await _context.Customer.Include(x => x.Accounts).
            //    FirstOrDefaultAsync(x => x.CustomerID == CustomerID);

            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Index(int AccountNumber, string ActionType)
        {
            
            var account = await _context.Account.FindAsync(AccountNumber);
            HttpContext.Session.SetInt32("CurrentAccount", account.AccountNumber);

            return RedirectToAction(ActionType);
        }


        public async Task<IActionResult> Deposit()
        {
            var id = HttpContext.Session.GetInt32("CurrentAccount");
            return View(await _context.Account.FindAsync(id));
        }

        // Before confirmation, validations needs to be done here.
        [HttpPost]
        public async Task<IActionResult> Deposit(int id, decimal amount)
        {
            var account = await _context.Account.FindAsync(id);

            //if (amount <= 0)
            //    ModelState.AddModelError(nameof(amount), "Amount must be positive.");
            //if (amount.HasMoreThanTwoDecimalPlaces())
            //    ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
            if (amount <= 0)
            {
                ViewBag.Amount = amount;
                return View(account);
            }

            // Note this code could be moved out of the controller, e.g., into the Model.
            account.Balance += amount;
            account.Transactions.Add(
                new Transaction
                {
                    TransactionType = 'D',
                    Amount = amount,
                    TransactionTimeUtc = DateTime.UtcNow
                });

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}

