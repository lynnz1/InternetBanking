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

        // Gets the selected action type, and redirect to the correspondign view page.
        // Deposit and WithDraw and both redirected to TransactionDetail view page.
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


        // Set up ViewModel for deposit and withdraw transactions.
        public async Task<IActionResult> TransactionDetail()
        {
            var account = await _context.Account.FindAsync(
                HttpContext.Session.GetInt32("CurrentAccount"));
            return View(TransactionViewSetUp(account));
        }

        // Set up ViewModel for transfer transactions.
        public async Task<IActionResult> Transfer()
        {
            var account = await _context.Account.FindAsync(
                HttpContext.Session.GetInt32("CurrentAccount"));
            return View(TransactionViewSetUp(account));
        }

        // Before confirmation, validations needs to be done here.
        // Amount and comment of deposit and withdraw actions are submitted here.
        // After validation the user is redirected to confirmation page.
        [HttpPost]
        public async Task<IActionResult> TransactionDetail(int id, decimal amount, string comment)
        {

            var account = await _context.Account.FindAsync(id);
            if (amount <= 0)
            {
                ModelState.AddModelError("Amount", "Invalid Amount");
                return View(TransactionViewSetUp(account));
            }
            if (HttpContext.Session.GetString("Transaction_ActionType").Equals("Withdraw"))
            {
                // insufficient Balance 
                if (account.NumOfTransactions >= 2 && account.AvailableBalance() < amount + 0.05m)
                {
                    ModelState.AddModelError("Amount", "Insufficient Balance");
                    return View(TransactionViewSetUp(account));
                    //return RedirectToAction("TransactionDetail");
                }
                if (account.NumOfTransactions < 2 && account.AvailableBalance() < amount)
                {
                    ModelState.AddModelError("Amount", "Insufficient Balance");
                    return View(TransactionViewSetUp(account));
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
                ModelState.AddModelError("Amount", "Invalid Destination Account");
                return View(TransactionViewSetUp(account));
            }

            // 
            // insufficient Balance 
            if (account.NumOfTransactions >= 2 && account.AvailableBalance() < amount + 0.10m)
            {
                ModelState.AddModelError("Amount", "Insufficient Balance");
                return View(TransactionViewSetUp(account));
            }
            if (account.NumOfTransactions < 2 && account.AvailableBalance() < amount)
            {
                ModelState.AddModelError("Amount", "Insufficient Balance");
                return View(TransactionViewSetUp(account));
            }
            // After validation. 

            // Redirect to confirmation
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

        // Push confirmed transaction data to database.
        [HttpPost]
        public async Task<IActionResult> Confirmation(TransactionViewModel transaction)
        {
            var account = await _context.Account.FindAsync(transaction.AccountNumber);


            if (transaction.ActionType == "Deposit")
            {
                // Reiceiving Transaction
                account.Balance += transaction.Amount;
                account.Transactions.Add(ReceivingTransaction(transaction));
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
                    account.Transactions.Add(ServiceFeeTransaction(transaction));
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
                desAccount.Transactions.Add(ReceivingTransaction(transaction));
                
                desAccount.Balance += transaction.Amount;
                // Service Fee Transaction
                if (account.NumOfTransactions >= 2)
                {
                    account.Transactions.Add(ServiceFeeTransaction(transaction));
                }
                account.NumOfTransactions++;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private TransactionViewModel TransactionViewSetUp(Account account)
        {
            TransactionViewModel t = new()
            {
                AccountNumber = account.AccountNumber,
                AccountType = account.AccountType,
                ActionType = HttpContext.Session.GetString("Transaction_ActionType")
            };
            return t;
        }

        private Transaction ReceivingTransaction (TransactionViewModel transaction)
        {
            Transaction t = new Transaction
            {
                TransactionType = transaction.ActionType.First(),
                Amount = transaction.Amount,
                TransactionTimeUtc = DateTime.UtcNow,
                Comment = transaction.Comment
            };
            return t;
        }

        private Transaction ServiceFeeTransaction(TransactionViewModel transaction)
        {
            Transaction t = new Transaction
            {
                TransactionType = 'S',
                Amount = transaction.ServiceFee,
                TransactionTimeUtc = DateTime.UtcNow,
            };
            return t;
        }

    }
}

