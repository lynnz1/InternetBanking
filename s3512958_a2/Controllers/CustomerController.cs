using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using s3512958_a2.Filters;
using s3512958_a2.Data;
using s3512958_a2.Models;
using Microsoft.EntityFrameworkCore;

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
            //var customer = await _context.Customers.FindAsync(CustomerID);

            // OR
            // Eager loading.
            var customer = await _context.Customer.Include(x => x.Accounts).
                FirstOrDefaultAsync(x => x.CustomerID == CustomerID);

            return View(customer);
        }

    }
}

