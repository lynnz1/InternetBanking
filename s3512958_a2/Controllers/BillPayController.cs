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
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace s3512958_a2.Controllers
{

    public class BillPayController : Controller
    {
        private readonly MyContext _context;

        public BillPayController(MyContext context) => _context = context;

        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public async Task<IActionResult> Index()
        {
            BillPayViewModel billPays = new();

            var customer = await _context.Customer.FindAsync(CustomerID);
            
            //  List<BillPay> Billpay. 
            foreach (var account in customer.Accounts)
            {
                // Get all the billpay rows with account number
                var bills = await _context.BillPay.Where(
                    x => x.AccountNumber == account.AccountNumber).ToListAsync();

                if (bills != null)
                {
                    // Loop through the billpay rows.
                    foreach (var bill in bills)
                    {
                        // Set the properties
                        billPays.Billpays.Add(
                            new BillPay
                            {
                                BillPayID = bill.BillPayID,
                                AccountNumber = account.AccountNumber,
                                PayeeID = bill.PayeeID,
                                Amount = bill.Amount,
                                ScheduleTimeUtc = bill.ScheduleTimeUtc,
                                Period = bill.Period
                            });
                        
                    }
                    
                }
            }

            return View(billPays);
        }

        
        public async Task<IActionResult> Create()
        {
            var customer = await _context.Customer.FindAsync(CustomerID);
            List<SelectListItem> accountItems = new List<SelectListItem>();
            // Account Select List
            foreach (var account in customer.Accounts)
            {
                accountItems.Add(new SelectListItem
                {
                    Text = account.AccountNumber.ToString(),
                    Value = account.AccountNumber.ToString()

                });
            }
            ViewBag.AccountList = accountItems;

            // Payee Select List
            List<SelectListItem> payeeItems = new List<SelectListItem>();
            var numOfPayee = await _context.Payee.CountAsync();
            for (int i = 0; i < numOfPayee; i++)
            {
                var payee = await _context.Payee.FindAsync(i + 1);
                payeeItems.Add(
                    new SelectListItem
                    {
                        Text = payee.Name,
                        Value = (i + 1).ToString()
                    });
            }
            ViewBag.PayeeList = payeeItems;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create (string AccountList, string PayeeList)
        {
            
            return View();
        }

        //public IActionResult Edit()
        //{
        //    HttpContext.Session.Clear();

        //    return RedirectToAction("Index", "Home");
        //}

        //public IActionResult Create()
        //{
        //    HttpContext.Session.Clear();

        //    return RedirectToAction("Index", "Home");
        //}
    }
}

