using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_client.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;

namespace MVC_client.Controllers
{
    public class HomeController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHostEnvironment _environment;

        public HomeController(IHttpClientFactory httpClientFactory, IHostEnvironment environment)
        {
            _httpClientFactory = httpClientFactory;
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Token()
        {
            if (_environment.IsDevelopment())
            {

                TokenViewModel tokenViewModel = new TokenViewModel();
                tokenViewModel.TimeStampLatestUpdate = new DateTime(InMemoryTokenRepo.TimestampLastAccessTokenUpdate).ToString("yyyy-MM-dd HH:mm:ss");

                var jwtAccessToken = new JwtSecurityTokenHandler().ReadJwtToken(InMemoryTokenRepo.AccessToken);
                tokenViewModel.AccessTokenHeader = JValue.Parse(jwtAccessToken.Header.SerializeToJson()).ToString(Formatting.Indented);
                tokenViewModel.AccessTokenPayload = JValue.Parse(jwtAccessToken.Payload.SerializeToJson()).ToString(Formatting.Indented);
                tokenViewModel.AccessToken_nbf = jwtAccessToken.ValidFrom.ToString("yyyy-MM-dd HH:mm:ss");
                tokenViewModel.AccessToken_exp = jwtAccessToken.ValidTo.ToString("yyyy-MM-dd HH:mm:ss");
                tokenViewModel.AccessToken_auth_time = jwtAccessToken.IssuedAt.ToString("yyyy-MM-dd HH:mm:ss");

                var jwtIdToken = new JwtSecurityTokenHandler().ReadJwtToken(InMemoryTokenRepo.IdToken);
                tokenViewModel.IdTokenHeader = JValue.Parse(jwtIdToken.Header.SerializeToJson()).ToString(Formatting.Indented);
                tokenViewModel.IdTokenPayload = JValue.Parse(jwtIdToken.Payload.SerializeToJson()).ToString(Formatting.Indented);
                tokenViewModel.IdToken_nbf = jwtIdToken.ValidFrom.ToString("yyyy-MM-dd HH:mm:ss");
                tokenViewModel.IdToken_exp = jwtIdToken.ValidTo.ToString("yyyy-MM-dd HH:mm:ss");
                tokenViewModel.IdToken_auth_time = jwtIdToken.IssuedAt.ToString("yyyy-MM-dd HH:mm:ss");

                if (!string.IsNullOrEmpty(InMemoryTokenRepo.RefreshToken))
                {
                    var jwtRefreshToken = new JwtSecurityTokenHandler().ReadJwtToken(InMemoryTokenRepo.RefreshToken);
                }








                return View("Token", tokenViewModel);
            }

            return Ok("");
        }

        //[Authorize(Policy="rc.scope")]
        [Authorize]
        public async Task<IActionResult> Secret()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            InMemoryTokenRepo.SetAccessToken(accessToken);
            var idToken = await HttpContext.GetTokenAsync("id_token"); //Används "internt" i is4
            InMemoryTokenRepo.SetIdToken(idToken);
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
            InMemoryTokenRepo.SetRefreshToken(refreshToken);

            //var claims = User.Claims.ToList();
            //var _accessToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            //var cookieClaimsIsh = _accessToken.Claims;
            //var _idToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);

            var result = await GetSecretFromApi1(accessToken);
            //var result2 = await GetSecretFromURLWithAccessToken("https://localhost:44383/policy", accessToken);
            //var result3 = await GetSecretFromURLWithAccessToken("https://localhost:44383/rolepolicy", accessToken);


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
