using Common;
using Common.Extensions;
using Microsoft.AspNetCore.Authentication;
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

        public RegisterController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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

            var IDPClient = _httpClientFactory.CreateClient().HttpClientPrep(uri.IDP, await HttpContext.GetTokenAsync("access_token"));
            await IDPClient.PostAsJsonAsync("api/V01/Register/Register", vm);

            return Redirect("https://localhost:44345/Dev/Devpage");
        }

    }
}

