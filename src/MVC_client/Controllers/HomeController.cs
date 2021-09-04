using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MVC_client.Controllers
{
    public class HomeController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[Authorize(Policy="rc.scope")]
        [Authorize]
        public async Task<IActionResult> Secret()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token"); //Används "internt" i is4
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            var claims = User.Claims.ToList();
            var _accessToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            var _idToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);

            var result = await GetSecretFromApi2(accessToken);

            return View();
        }

        //mvc_client, access the api2 resorce
        public async Task<string> GetSecretFromApi2(string accessToken)
        {
            //Use token, and get secret data from API1. Dvs vi bygger en http request, med en attatchad bearer token.
            var CallAPI2Client = _httpClientFactory.CreateClient();
            CallAPI2Client.SetBearerToken(accessToken);
            var SecretResponse = await CallAPI2Client.GetAsync("https://localhost:44383/secret");
            var SecretMessage = await SecretResponse.Content.ReadAsStringAsync();
            return SecretMessage;
        }


    }
}
