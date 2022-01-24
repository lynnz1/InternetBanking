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
            var customer = await _context.Customer.FindAsync(CustomerID);
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Index(int AccountNumber, string ActionType)
        {
            
            var account = await _context.Account.FindAsync(AccountNumber);
            
            HttpContext.Session.SetInt32("CurrentAccount", account.AccountNumber);
            HttpContext.Session.SetString("Transaction_ActionType", ActionType);

            return RedirectToAction("TransactionDetail");
        }


        public IActionResult TransactionDetail()
        {
            var id = HttpContext.Session.GetInt32("CurrentAccount");
            TransactionViewModel t = new TransactionViewModel
            {
                AccountNumber = (int)id,
                ActionType = HttpContext.Session.GetString("Transaction_ActionType")
            };
            return View(t);
        }

        // Before confirmation, validations needs to be done here.
        [HttpPost]
        public async Task<IActionResult> TransactionDetail(int id, decimal amount, string comment)
        {
            // After validation. 
            var account = await _context.Account.FindAsync(id);

            HttpContext.Session.SetInt32("Transaction_AccountNumber",id);
            AmountCommentViewModel amountComment = new AmountCommentViewModel
            {
                Amount = amount,
                Comment = comment,
                ActionType = HttpContext.Session.GetString("Transaction_ActionType")
            };
            
            return RedirectToAction("Confirmation",amountComment);

        }

        public async Task<IActionResult> Confirmation(AmountCommentViewModel amountComment)
        {
            var accountNum = HttpContext.Session.GetInt32("Transaction_AccountNumber");
            var account = await _context.Account.FindAsync(accountNum);
            TransactionViewModel transaction = new TransactionViewModel()
            {
                AccountNumber = account.AccountNumber,
                ActionType = amountComment.ActionType,
                Amount = amountComment.Amount,
                Comment = amountComment.Comment,
                AccountType = account.AccountType,
                ServiceFee = 0
            };
            return View(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> Confirmation(TransactionViewModel transaction)
        {
            var account = await _context.Account.FindAsync(transaction.AccountNumber);
            
            //if (amount <= 0)
            //    ModelState.AddModelError(nameof(amount), "Amount must be positive.");
            //if (amount.HasMoreThanTwoDecimalPlaces())
            //    ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
            if (transaction.Amount <= 0)
            {
                ViewBag.Amount = transaction.Amount;
                return View(account);
            }

            // Note this code could be moved out of the controller, e.g., into the Model.
            if (transaction.ActionType == "Deposit")
            {
                account.Balance += transaction.Amount;
                account.Transactions.Add(
                    new Transaction
                    {
                        TransactionType = transaction.ActionType.First(),
                        Amount = transaction.Amount,
                        TransactionTimeUtc = DateTime.UtcNow,
                        Comment = transaction.Comment
                    });
            }
            else
            {
                account.Balance -= transaction.Amount;
                account.NumOfTransactions++;
                account.Transactions.Add(
                    new Transaction
                    {
                        TransactionType = transaction.ActionType.First(),
                        Amount = transaction.Amount,
                        TransactionTimeUtc = DateTime.UtcNow,
                        Comment = transaction.Comment
                    });
            }
            

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}

