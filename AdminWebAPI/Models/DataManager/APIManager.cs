using System;
using AdminWebAPI.Data;
using AdminWebAPI.Models.Repository;

namespace AdminWebAPI.Models.DataManager
{
    public class APIManager
    {
        private readonly MyContext _context;

        public APIManager(MyContext context) => _context = context;

        public AdminLogin GetAdminLogin()
        {
            AdminLogin adminLogin = new()
            {
                Username = "admin",
                PasswordHash = SimpleHashing.PBKDF2.Hash("admin")
            };
            return adminLogin;
        }

        //
        public IEnumerable<Transaction> GetTransaction(int id, DateTime? startDate, DateTime? endDate)
        {
            // Filter transaction result
            if (startDate != null && endDate != null)
            {
                startDate = startDate?.ToUniversalTime();
                endDate = endDate?.ToUniversalTime();
                var allTransactions = _context.Transaction
                    .Where(x => x.AccountNumber == id).ToList();
                var filteredTransactions = allTransactions
                    .Where(x => x.TransactionTimeUtc >= startDate && x.TransactionTimeUtc <= endDate).ToList();
                
                return filteredTransactions;
            }
            // if no start date and / or end date is entered then no filter is applied
            else
            {

                return _context.Transaction.Where(x => x.AccountNumber == id).ToList();
            }
            
            
        }

        public IEnumerable<Account> GetAll()
        {
            return _context.Account.ToList();
        }

        public IEnumerable<Customer> GetAllCustomer()
        {
            return _context.Customer.ToList();
        }

        public IEnumerable<BillPay> GetBillPay(int id)
        {
            return _context.BillPay.Where(x => x.AccountNumber == id).ToList();
        }

        public int BlockBillPay(int id)
        {
            var bill = _context.BillPay.Find(id);
            bill.IsBlocked = true;
            _context.Update(bill);
            _context.SaveChanges();

            return id;
        }
        public int UnBlockBillPay(int id)
        {
            var bill = _context.BillPay.Find(id);
            bill.IsBlocked = false;
            _context.Update(bill);
            _context.SaveChanges();

            return id;
        }

        public int UpdateCustomer(int id, Customer customer)
        {
            _context.Update(customer);
            _context.SaveChanges();

            return id;
        }

        public int LockLogin(int id)
        {
            var login = _context.Login.Where(x => x.CustomerID == id).First();
            login.IsLocked = true;
            _context.Update(login);
            _context.SaveChanges();

            return id;
        }
        public int UnLockLogin(int id)
        {
            var login = _context.Login.Where(x => x.CustomerID == id).First();
            login.IsLocked = false;
            _context.Update(login);
            _context.SaveChanges();

            return id;
        }

        public BillPay GetBill (int id)
        {
            return _context.BillPay.Find(id);
        }

        public Customer GetCustomer (int id)
        {
            return _context.Customer.Find(id);
        }

        public Login GetLogin(int id)
        {
            return _context.Login.Where(x => x.CustomerID == id).First();
        }

    }
}

