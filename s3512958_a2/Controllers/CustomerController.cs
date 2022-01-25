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

            if (ActionType.Equals("Transfer"))
            {
                return RedirectToAction("Transfer");
            }
            return RedirectToAction("TransactionDetail");
        }



        public async Task<IActionResult> TransactionDetail()
        {
            var account = await _context.Account.FindAsync(
                HttpContext.Session.GetInt32("CurrentAccount"));
            var id = HttpContext.Session.GetInt32("CurrentAccount");
            TransactionViewModel t = new()
            {
                AccountNumber = account.AccountNumber,
                AccountType = account.AccountType,
                ActionType = HttpContext.Session.GetString("Transaction_ActionType")
            };
            return View(t);
        }


        public async Task<IActionResult> Transfer()
        {
            var account = await _context.Account.FindAsync(
                HttpContext.Session.GetInt32("CurrentAccount"));
            var id = HttpContext.Session.GetInt32("CurrentAccount");
            TransactionViewModel t = new()
            {
                AccountNumber = account.AccountNumber,
                AccountType = account.AccountType,
                ActionType = HttpContext.Session.GetString("Transaction_ActionType")
            };
            return View(t);
        }

        // Before confirmation, validations needs to be done here.
        [HttpPost]
        public async Task<IActionResult> TransactionDetail(int id, decimal amount, string comment)
        {

            var account = await _context.Account.FindAsync(id);

            if (HttpContext.Session.GetString("Transaction_ActionType").Equals("Withdraw"))
            {
                if (account.NumOfTransactions >= 2 && account.AvailableBalance() < amount + 0.05m)
                {
                    return RedirectToAction("TransactionDetail");
                }
            }
            // After validation. 


            AmountCommentViewModel amountComment = new()
            {
                Amount = amount,
                Comment = comment,
                ActionType = HttpContext.Session.GetString("Transaction_ActionType")

            };

            return RedirectToAction("Confirmation", amountComment);

        }

        [HttpPost]
        public async Task<IActionResult> Transfer(int id, decimal amount, string Comment, int DesAccount)
        {

            var account = await _context.Account.FindAsync(id);

            // Destination Account Incorrect
            if (!_context.Account.Any(a => a.AccountNumber == DesAccount) || DesAccount == id)
            {
                return RedirectToAction("Transfer");
            }

            if (account.NumOfTransactions >= 2 && account.AvailableBalance() < amount + 0.10m)
            {
                return RedirectToAction("Transfer");
            }
            // After validation. 


            AmountCommentViewModel amountComment = new()
            {
                Amount = amount,
                Comment = Comment,
                ActionType = HttpContext.Session.GetString("Transaction_ActionType"),
                DesAccount = DesAccount
            };

            return RedirectToAction("Confirmation", amountComment);

        }

        public async Task<IActionResult> Confirmation(AmountCommentViewModel amountComment)
        {
            var accountNum = HttpContext.Session.GetInt32("CurrentAccount");
            var account = await _context.Account.FindAsync(accountNum);
            TransactionViewModel transaction = new()
            {
                AccountNumber = account.AccountNumber,
                ActionType = amountComment.ActionType,
                Amount = amountComment.Amount,
                Comment = amountComment.Comment,
                AccountType = account.AccountType,
                DesAccount = amountComment.DesAccount

            };
            transaction.ServiceFee = transaction.GetServiceFee(account.NumOfTransactions);

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
            if (transaction.ActionType == "Withdraw")
            {
                account.Balance -= transaction.Amount + transaction.ServiceFee;
                // Withdraw Transaction
                account.Transactions.Add(
                    new Transaction
                    {
                        TransactionType = transaction.ActionType.First(),
                        Amount = transaction.Amount,
                        TransactionTimeUtc = DateTime.UtcNow,
                        Comment = transaction.Comment
                    });
                // Service Fee Transaction
                if (account.NumOfTransactions >= 2)
                {
                    account.Transactions.Add(
                    new Transaction
                    {
                        TransactionType = 'S',
                        Amount = transaction.ServiceFee,
                        TransactionTimeUtc = DateTime.UtcNow,
                    });
                }
                account.NumOfTransactions++;
            }
            if (transaction.ActionType == "Transfer")
            {
                var desAccount = await _context.Account.FindAsync(transaction.DesAccount);


                // Transfer Transaction
                account.Transactions.Add(
                    new Transaction
                    {
                        TransactionType = transaction.ActionType.First(),
                        DestinationAccountNumber = transaction.DesAccount,
                        Amount = transaction.Amount,
                        TransactionTimeUtc = DateTime.UtcNow,
                        Comment = transaction.Comment
                    });
                account.Balance -= transaction.Amount + transaction.ServiceFee;
                // Receiving Transaction
                desAccount.Transactions.Add(
                    new Transaction
                    {
                        TransactionType = transaction.ActionType.First(),
                        Amount = transaction.Amount,
                        TransactionTimeUtc = DateTime.UtcNow,
                        Comment = transaction.Comment
                    });
                desAccount.Balance += transaction.Amount;
                // Service Fee Transaction
                if (account.NumOfTransactions >= 2)
                {
                    account.Transactions.Add(
                    new Transaction
                    {
                        TransactionType = 'S',
                        Amount = transaction.ServiceFee,
                        TransactionTimeUtc = DateTime.UtcNow,
                    });
                }
                account.NumOfTransactions++;
            }




            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}

