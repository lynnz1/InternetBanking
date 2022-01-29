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

namespace s3512958_a2.Controllers
{
    [AuthorizeCustomer]
    public class MyProfileController : Controller
    {
        private readonly MyContext _context;

        // ReSharper disable once PossibleInvalidOperationException
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public MyProfileController(MyContext context) => _context = context;


        public async Task<IActionResult> Index()
        {
            var customer = await _context.Customer.FindAsync(CustomerID);
            return View(customer);
        }

        public async Task<IActionResult> Edit()
        {
            var customer = await _context.Customer.FindAsync(CustomerID);
            return View(customer);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Customer customer)
        {
            // ReSharper disable once InvertIf
            _context.Update(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult UpdatePassword()
        {
            return View("UpdatePassword");
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(string password)
        {
            
            var customer = await _context.Customer.FindAsync(CustomerID);
            customer.Login.PasswordHash = PBKDF2.Hash(password);
            await _context.SaveChangesAsync();
            return RedirectToAction("Logout", "Login");
        }



    }

}

