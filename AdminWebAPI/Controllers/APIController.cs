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

        // Return specific billpay
        [HttpGet("BillPay/{id}")]
        public BillPay GetBillPay(int id)
        {
            return _repo.GetBill(id);
        }

        [HttpGet("Customer/{id}")]
        public Customer GetCustomer(int id)
        {
            return _repo.GetCustomer(id);
        }

        [HttpGet("Login/{id}")]
        public Login GetLogin(int id)
        {
            return _repo.GetLogin(id);
        }

        // id = BillPayID
        [HttpPut("BlockBillPay")]
        public void BlockBillPay([FromBody] int id)
        {
            _repo.BlockBillPay(id);
        }
        [HttpPut("UnBlockBillPay")]
        public void UnBlockBillPay([FromBody] int id)
        {
            _repo.UnBlockBillPay(id);
        }

        // id = Customer ID
        [HttpPut("LockLogin")]
        public void LockLogin ([FromBody] int id)
        {
            _repo.LockLogin(id);
        }
        [HttpPut("UnLockLogin")]
        public void UnLockLogin([FromBody] int id)
        {
            _repo.UnLockLogin(id);
        }

        [HttpPut("UpdateCustomer")]
        public void PutCustomer([FromBody] Customer customer)
        {
            _repo.UpdateCustomer(customer.CustomerID, customer);
        }

    }
}

