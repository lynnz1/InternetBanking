using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Admin.Models;
using Newtonsoft.Json;
using System.Text;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Admin.Controllers
{
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

        public async Task<IActionResult> Transaction(int id)
        {
            if (id == null)
                return NotFound();

            var response = await Client.GetAsync($"api/Transactions/{id}");

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            var result = await response.Content.ReadAsStringAsync();
            if (result.Count() == 0)
            {
                return View();
            }
            var transactions = JsonConvert.DeserializeObject<List<Transaction>>(result);

            return View(transactions);
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

        public async Task<IActionResult> Block (int id)
        {
            var response = await Client.GetAsync($"api/BillPay/{id}");
            if (!response.IsSuccessStatusCode)
                throw new Exception();

            var result = await response.Content.ReadAsStringAsync();
            
            var billPay = JsonConvert.DeserializeObject<BillPay>(result);
            billPay.IsBlocked = true;
            var content = new StringContent(JsonConvert.SerializeObject(billPay), Encoding.UTF8, "application/json");
            var putResponse = Client.PutAsync("api/UpdateBillPay", content).Result;
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

