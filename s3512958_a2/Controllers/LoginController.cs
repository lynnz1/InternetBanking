using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InternetBanking.Data;
using InternetBanking.Models;
using InternetBanking.Filters;
using Microsoft.EntityFrameworkCore;
using SimpleHashing;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InternetBanking.Controllers
{
   
    public class LoginController : Controller
    {
        private readonly MyContext _context;

        public LoginController(MyContext context) => _context = context;

        public IActionResult Login()
        {
            // Redirect Logged in users to their customer page
            var customerID = HttpContext.Session.GetInt32(nameof(Customer.CustomerID));
            if (customerID.HasValue)
            {
                return RedirectToAction("Index", "Customer");
            }

            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Login(string LoginID, string password)
        {
            //var login = await _context.Login.FindAsync(LoginID);
            var login = await _context.Login.Include(x => x.Customer).FirstOrDefaultAsync(x => x.LoginID==LoginID);
            if (login.IsLocked == true)
            {
                ModelState.AddModelError("LoginFailed", "You account has been locked.");
                return View();
            }
            if (login == null || string.IsNullOrEmpty(password) || !PBKDF2.Verify(login.PasswordHash, password))
            {
                ModelState.AddModelError("LoginFailed", "Login failed, please try again.");
                return View(new Login { LoginID = LoginID });
            }

            // Login customer.
            HttpContext.Session.SetInt32(nameof(Customer.CustomerID), login.CustomerID);
            HttpContext.Session.SetString(nameof(Customer.Name), login.Customer.Name);
            HttpContext.Session.SetInt32("CurrentAccount", 0);

            return RedirectToAction("Index", "Customer");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}

