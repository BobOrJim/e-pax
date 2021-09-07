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
            var cookieClaimsIsh = _accessToken.Claims;
            var _idToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);

            var result = await GetSecretFromApi1(accessToken);
            var result2 = await GetSecretFromURLWithAccessToken("https://localhost:44383/policy", accessToken);
            var result3 = await GetSecretFromURLWithAccessToken("https://localhost:44383/rolepolicy", accessToken);


            return View();
        }

        public async Task<string> GetSecretFromURLWithAccessToken(string url, string accessToken)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.SetBearerToken(accessToken);
            var SecretResponse = await httpClient.GetAsync(url);
            return await SecretResponse.Content.ReadAsStringAsync();
        }

        //mvc_client, access the api1 resorce
        public async Task<string> GetSecretFromApi1(string accessToken)
        {
            //Use token, and get secret data from API1. Dvs vi bygger en http request, med en attatchad bearer token.
            var CallAPIClient = _httpClientFactory.CreateClient();
            CallAPIClient.SetBearerToken(accessToken);
            var SecretResponse = await CallAPIClient.GetAsync("https://localhost:44383/secret");
            var SecretMessage = await SecretResponse.Content.ReadAsStringAsync();
            return SecretMessage;
        }





    }
}
