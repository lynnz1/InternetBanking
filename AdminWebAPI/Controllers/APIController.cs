using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdminWebAPI.Models;
using AdminWebAPI.Models.DataManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdminWebAPI.Controllers
{

    [Route("[controller]")]
    public class APIController : Controller
    {
        private readonly APIManager _repo;

        public APIController(APIManager repo)
        {
            _repo = repo;
        }

        // Get admin username and hashed password
        [HttpGet("AdminLogin")]
        public AdminLogin GetAdminLogin()
        {
            return _repo.GetAdminLogin();
        }

        // Return all accounts
        [HttpGet("Accounts")]
        public IEnumerable<Account> GetAllAccounts()
        {
            return _repo.GetAll();
        }
        // Return transaction of an account, id = accountNumber
        // GET api/account/1(AccountID)(Showing all transactions of this account)
        [HttpGet("Transactions/{id}")]
        public IEnumerable<Transaction> GetTransactions (int id)
        {
            return _repo.GetTransaction(id);
        }

        // return billpay of an account, id = accountNumber
        [HttpGet("BillPays/{id}")]
        public IEnumerable<BillPay> GetBillPays(int id)
        {
            return _repo.GetBillPay(id);
        }

        // Return all customers
        [HttpGet("Customers")]
        public IEnumerable<Customer> GetAllCustomers()
        {
            return _repo.GetAllCustomer();
        }

        [HttpPut("UpdateBillPay")]
        public void PutBillPay([FromBody] BillPay billPay)
        {
            _repo.UpdateBillPay(billPay.BillPayID, billPay);
        }

        [HttpPut("UpdateLogin")]
        public void PutLogin ([FromBody] Login login)
        {
            _repo.UpdateLogin(login.LoginID, login);
        }

        [HttpPut("UpdateCustomer")]
        public void PutCustomer([FromBody] Customer customer)
        {
            _repo.UpdateCustomer(customer.CustomerID, customer);
        }

    }
}

