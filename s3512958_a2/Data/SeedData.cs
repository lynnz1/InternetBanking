using Newtonsoft.Json;
using s3512958_a2.Models;
namespace s3512958_a2.Data;

public static class SeedData
{
    public static void Preload(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<MyContext>();
        // Look for customers.
        if (context.Customer.Any())
            return; // DB has already been seeded.


        const string Url = "https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/";


        using var client = new HttpClient();
        var json = client.GetStringAsync(Url).Result;


        var customer = JsonConvert.DeserializeObject<List<Customer>>(
            json,
            new JsonSerializerSettings
            {
                DateFormatString = "dd/MM/yyyy hh:mm:ss tt",
                Formatting = Formatting.Indented

            }
            );

        foreach (var c in customer)
        {
            InsertIntoCustomer(context, c);
            InsertIntoLogin(context, c);

            foreach (var a in c.Accounts)
            {
                var balance = a.CalculateBalance();
                a.Balance = balance;

                InsertIntoAccount(context, a);

                foreach (var t in a.Transactions)
                {
                    t.TransactionType = 'D';
                    t.AccountNumber = a.AccountNumber;
                    InsertIntoTransactions(context, t);
                }
            }


        }
        context.SaveChanges();
    }
    private static void InsertIntoCustomer(MyContext context, Customer c)
    {
        context.Customer.Add(
            new Customer
            {
                CustomerID = c.CustomerID,
                Name = c.Name,
                Address = c.Address,
                Suburb = c.Suburb,
                PostCode = c.PostCode
            });
    }
    private static void InsertIntoLogin(MyContext context, Customer c)
    {
        context.Login.Add(
            new Login
            {
                LoginID = c.Login.LoginID,
                CustomerID = c.CustomerID,
                PasswordHash = c.Login.PasswordHash
            });
    }
    private static void InsertIntoAccount(MyContext context, Account a)
    {
        context.Account.Add(
            new Account
            {
                AccountNumber = a.AccountNumber,
                AccountType = a.AccountType,
                CustomerID = a.CustomerID,
                Balance = a.Balance,
                NumOfTransactions = 0
            });
    }
    private static void InsertIntoTransactions(MyContext context, Transaction t)
    {
        const string format = "dd/MM/yyyy hh:mm:ss tt";
        context.Transaction.Add(
            new Transaction
            {
                TransactionType = t.TransactionType,
                AccountNumber = t.AccountNumber,
                Amount = t.Amount,
                Comment = t.Comment,
                TransactionTimeUtc = t.TransactionTimeUtc
            });
    }



}
