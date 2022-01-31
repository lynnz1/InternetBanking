using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Admin.Models;
using Newtonsoft.Json;
using System.Text;
using Admin.Filters;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Admin.Controllers
{
    [AuthorizeCustomer]
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private HttpClient Client => _clientFactory.CreateClient("api");

        public AccountController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var response = await Client.GetAsync("api/Accounts");
            //var response = await MovieApi.InitializeClient().GetAsync("api/movies");

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            // Storing the response details received from web api.
            var result = await response.Content.ReadAsStringAsync();

            // Deserializing the response received from web api and storing into a list.
            var accounts = JsonConvert.DeserializeObject<List<Account>>(result);

            return View(accounts);
        }

        public async Task<IActionResult> Transaction(int id, FilterDateViewModel dates)
        {
            if (id != 0)
            {
                dates.AccountNumber = id;
            }
            // ?startDate=01-01-2022&endDate=01-01-2023
            var response = await Client.GetAsync($"api/Transactions/" +
                $"{dates.AccountNumber}?startDate={dates.Start}&endDate={dates.End}");

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            var result = await response.Content.ReadAsStringAsync();
            // If null is returned from API, Display all transactions
            if (result.Equals("[]"))
            {
                dates.Start = null;
                dates.End = null;
                return RedirectToAction("Transaction", dates);
            }
            var transactions = JsonConvert.DeserializeObject<List<Transaction>>(result);
            dates.Transactions = transactions;
            dates.AccountNumber = transactions[0].AccountNumber;
            return View(dates);
        }
        [HttpPost]
        public async Task<IActionResult> FilterTransaction(FilterDateViewModel dates)
        {
            return RedirectToAction("Transaction", dates);
        }



        public async Task<IActionResult> BillPay (int id)
        {
            if (id == null)
                return NotFound();

            var response = await Client.GetAsync($"api/BillPays/{id}");

            if (!response.IsSuccessStatusCode)
                throw new Exception();
            
            var result = await response.Content.ReadAsStringAsync();
            if (result.Count() == 0)
            {
                return View();
            }
            var transactions = JsonConvert.DeserializeObject<List<BillPay>>(result);

            return View(transactions);
            
        }

        public async Task<IActionResult> Block (int id, string action)
        {
            HttpResponseMessage putResponse;
            var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            if (action.Equals("block"))
            {
                putResponse = Client.PutAsync("api/BlockBillPay", content).Result;
            }
            else
            {
                putResponse = Client.PutAsync("api/UnBlockBillPay", content).Result;
            }

            if (putResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                throw new Exception();
            }
        }
    }
}

