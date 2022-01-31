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
    public class CustomerController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private HttpClient Client => _clientFactory.CreateClient("api");

        public CustomerController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        // Get all customers
        public async Task<IActionResult> Index()
        {
            var response = await Client.GetAsync("api/Customers");
            //var response = await MovieApi.InitializeClient().GetAsync("api/movies");

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            // Storing the response details received from web api.
            var result = await response.Content.ReadAsStringAsync();

            // Deserializing the response received from web api and storing into a list.
            var customers = JsonConvert.DeserializeObject<List<Customer>>(result);

            return View(customers);
        }

        public async Task<IActionResult> LoginInfo(int id)
        {
            var response = await Client.GetAsync($"api/Login/{id}");
            //var response = await MovieApi.InitializeClient().GetAsync("api/movies");

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            // Storing the response details received from web api.
            var result = await response.Content.ReadAsStringAsync();

            // Deserializing the response received from web api and storing into a list.
            var login = JsonConvert.DeserializeObject<Login>(result);

            return View(login);
        }

        [HttpPost]
        public IActionResult Lock(int id, string action)
        {
            HttpResponseMessage putResponse;
            var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
            if (action.Equals("lock"))
            {
                putResponse = Client.PutAsync("api/LockLogin", content).Result;
            }
            else
            {
                putResponse = Client.PutAsync("api/UnLockLogin", content).Result;
            }
            
            if (putResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("LoginInfo");
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await Client.GetAsync($"api/Customer/{id}");

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            // Storing the response details received from web api.
            var result = await response.Content.ReadAsStringAsync();

            // Deserializing the response received from web api and storing into a list.
            var customer = JsonConvert.DeserializeObject<Customer>(result);

            return View(customer);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(Customer customer)
        {
            var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
           
            
                var putResponse = Client.PutAsync("api/UpdateCustomer", content).Result;
            

            if (!putResponse.IsSuccessStatusCode)
            {
                throw new Exception();
            }
            

            return RedirectToAction("Index");
        }
    }
}

