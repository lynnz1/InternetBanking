//using Newtonsoft.Json;
//using s3512958_a2.Models;
//namespace s3512958_a2.Data
//{
//	public static class DeserializeJSON
//	{
//		private const string ConnectionString =
//		"server=rmit.australiaeast.cloudapp.azure.com;TrustServerCertificate=True;" +
//		"uid=s3512958_a1;database=s3512958_a1;pwd=abc123";

//        public static void Preload()
//        {

//            const string Url = "https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/";

//            using var client = new HttpClient();
//            var json = client.GetStringAsync(Url).Result;


//            var customer = JsonConvert.DeserializeObject<List<Customer>>(
//                json,
//                new JsonSerializerSettings
//                {
//                    DateFormatString = "dd/MM/yyyy hh:mm:ss tt",
//                    Formatting = Formatting.Indented

//                }
//                );

//            foreach (var c in customer)
//            {
//                InsertIntoCustomer(c);
//                InsertIntoLogin(c);

//                foreach (var a in c.Accounts)
//                {
//                    var balance = a.CalculateBalance();
//                    a.Balance = balance;

//                    InsertIntoAccount(a);

//                    foreach (var t in a.Transactions)
//                    {
//                        t.TransactionType = 'D';
//                        t.AccountNumber = a.AccountNumber;

//                        ;

//                        InsertIntoTransactions(t);
//                    }
//                }


//            }
//        }
//    }
//}

