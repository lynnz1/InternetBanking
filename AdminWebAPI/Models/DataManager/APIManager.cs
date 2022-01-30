﻿using System;
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

        public IEnumerable<Transaction> GetTransaction(int id)
        {
            return _context.Transaction.Where(x => x.AccountNumber == id).ToList();
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

        public int UpdateBillPay(int id, BillPay billPay)
        {
            _context.Update(billPay);
            _context.SaveChanges();

            return id;
        }

        public int UpdateCustomer(int id, Customer customer)
        {
            _context.Update(customer);
            _context.SaveChanges();

            return id;
        }

        public string UpdateLogin(string id, Login login)
        {
            _context.Update(login);
            _context.SaveChanges();

            return id;
        }

    }
}

