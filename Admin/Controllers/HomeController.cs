using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Admin.Models;
using Newtonsoft.Json;
using SimpleHashing;

namespace Admin.Controllers;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _clientFactory;
    private HttpClient Client => _clientFactory.CreateClient("api");

    public HomeController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;


    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(AdminLogin adminLogin, string password)
    {
        if (adminLogin.Username == null || password == null)
        {
            return RedirectToAction("index");
        }
        var response = await Client.GetAsync("API/AdminLogin");

        if (!response.IsSuccessStatusCode)
            throw new Exception();

        // Storing the response details received from web api.
        var result = await response.Content.ReadAsStringAsync();

        // Deserializing the response received from web api and storing into a list.
        var login = JsonConvert.DeserializeObject<AdminLogin>(result);
        if (login.Username.Equals(adminLogin.Username) && PBKDF2.Verify(login.PasswordHash, password))
        {

            return RedirectToAction("Index", "Account");
        }
        else
        {
            return RedirectToAction("index");
        }

    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

