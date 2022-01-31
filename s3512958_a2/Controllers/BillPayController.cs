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

namespace s3512958_a2.Controllers
{
    [AuthorizeCustomer]
    public class BillPayController : Controller
    {
        private readonly MyContext _context;

        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public BillPayController(MyContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            BillPayViewModel billPays = new()
            {
                Billpays = new List<BillPay>()

            };
            

            var customer = await _context.Customer.Include(x => x.Accounts).
                FirstOrDefaultAsync(x => x.CustomerID == CustomerID);


            //  List<BillPay> Billpay. 
            foreach (var account in customer.Accounts)
            {
                // Get all the billpay rows with account number
                var bills = await _context.BillPay.Where(x => x.AccountNumber == account.AccountNumber).ToListAsync();

                if (bills.Count != 0)
                {
                    // Loop through the billpay rows.
                    foreach (var bill in bills)
                    {
                        var payee = await _context.Payee.FirstOrDefaultAsync(x => x.PayeeID == bill.PayeeID);
                       
                        BillPay billpay = new BillPay
                        {
                            BillPayID = bill.BillPayID,
                            AccountNumber = account.AccountNumber,
                            PayeeID = bill.PayeeID,
                            Payee = payee,
                            Amount = bill.Amount,
                            ScheduleTimeUtc = bill.ScheduleTimeUtc,
                            Period = bill.Period,
                            IsBlocked = bill.IsBlocked
                        };

                        // Set the properties
                        billPays.Billpays.Add(billpay);

                    }

                }
            }

            return View(billPays);
        }

        
        [HttpPost]
        public IActionResult Index(string BillPayID, string Action)
        {
            HttpContext.Session.SetInt32("BillPayID", int.Parse(BillPayID));
            if (Action.Equals("Edit"))
            {
                return RedirectToAction("Edit");
            }
            else
            {
                return RedirectToAction("Delete");
            }
        }


        public async Task<IActionResult> Create()
        {
            BillPayViewModel billPay = new();
            var customer = await _context.Customer.FindAsync(CustomerID);

            // Account Select List
            billPay.AccountSelectList = PopulateSelectLists.AccountSelectList(customer);

            // Payee Select List
            var allPayee = await _context.Payee.ToListAsync();
            billPay.PayeeSelectList = PopulateSelectLists.PayeeSelectList(allPayee);
            
            billPay.Period = PopulateSelectLists.PeriodSelectList();
            return View(billPay);
        }

        // Push new billpay to database
        [HttpPost]
        public async Task<IActionResult> Create(BillPayViewModel billPayViewModel)
        {
            BillPay billPay = new();
            if (billPayViewModel.Billpays[0].ScheduleTimeUtc.ToLocalTime() <= DateTime.Now)
            {
                return RedirectToAction("Create");
            }
            if (billPayViewModel.Billpays[0].Amount <= 0)
            {
                return RedirectToAction("Create");
            }
            _context.BillPay.Add(
                new BillPay
                {
                    AccountNumber = billPayViewModel.SelectedAccount,
                    PayeeID = billPayViewModel.SelectedPayee,
                    Amount = billPayViewModel.Billpays[0].Amount,
                    ScheduleTimeUtc = billPayViewModel.Billpays[0].ScheduleTimeUtc.ToUniversalTime(),
                    Period = billPayViewModel.SelectedPeriod
                });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        
        public async Task<IActionResult> Edit()
        {
            var billID = HttpContext.Session.GetInt32("BillPayID");
            var bill = await _context.BillPay.FindAsync(billID);
            bill.ScheduleTimeUtc = bill.ScheduleTimeUtc.ToLocalTime();
            BillPayViewModel billPays = new()
            {
                Billpays = new List<BillPay>()

            };
            billPays.Billpays.Add(bill);
            
            billPays.SelectedBillPayID = bill.BillPayID;
            var customer = await _context.Customer.FindAsync(CustomerID);
            var allPayee = await _context.Payee.ToListAsync();
            // Setup selectlists
            billPays.AccountSelectList = PopulateSelectLists.AccountSelectList(customer);
            billPays.PayeeSelectList = PopulateSelectLists.PayeeSelectList(allPayee);
            billPays.Period = PopulateSelectLists.PeriodSelectList();
            return View(billPays);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BillPayViewModel billPay)
        {
            // Scheduled time can not be in the past
            if (billPay.Billpays[0].ScheduleTimeUtc.ToLocalTime() <= DateTime.Now)
            {
                return RedirectToAction("Edit");
            }
            if (billPay.Billpays[0].Amount <= 0)
            {
                return RedirectToAction("Edit");
            }
            BillPay bill = new BillPay()
            {
                BillPayID = billPay.SelectedBillPayID,
                AccountNumber = billPay.SelectedAccount,
                PayeeID = billPay.SelectedPayee,
                Amount = billPay.Billpays[0].Amount,
                ScheduleTimeUtc = billPay.Billpays[0].ScheduleTimeUtc.ToUniversalTime(),
                Period = billPay.SelectedPeriod

            };
            
            _context.Update(bill);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
            
        }
        public async Task<IActionResult> Delete()
        {
            var billID = HttpContext.Session.GetInt32("BillPayID");
            var billInfo = await _context.BillPay.FindAsync(billID);
            return View(billInfo);

        }
        [HttpPost]
        public async Task<IActionResult> Delete(string BillPayID)
        {
            var bill = await _context.BillPay.FindAsync(int.Parse(BillPayID));
            _context.BillPay.Remove(bill);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }

    }
}

