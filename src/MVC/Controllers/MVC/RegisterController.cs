using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVC.ViewModels;
using System;
using System.Net.Http;
using System.Threading.Tasks;


namespace MVC.Controllers
{
    [Route("[controller]")]
    public class RegisterController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHostEnvironment _environment;

        public RegisterController(IHttpClientFactory httpClientFactory, IHostEnvironment environment)
        {
            _httpClientFactory = httpClientFactory;
            _environment = environment;
        }


        [HttpGet("Register")]
        public async Task<IActionResult> Register()
        {
            return View("Register");
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", vm);
            }

            var IDPClient = _httpClientFactory.CreateClient();
            IDPClient.BaseAddress = new Uri("https://localhost:44327/");
            await IDPClient.PostAsJsonAsync("api/V01/Register/Register", vm);

            return Redirect("https://localhost:44345/Dev/Devpage");
        }

    }
}

