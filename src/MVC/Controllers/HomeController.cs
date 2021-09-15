﻿using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.ViewModels;
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

namespace MVC.Controllers
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
        public IActionResult Logout()
        {
            return SignOut("mvc_client_cookie", "oidc"); //
        }
        public async Task<IActionResult> Token()
        {
            await Task.CompletedTask;
            return View("Token", TokenViewModelFactory());
        }
        [Authorize]
        public async Task<IActionResult> MVCSecret()
        {
            CheckIfRefreshTokenShouldBeUsed().GetAwaiter().GetResult();
            UpdateInMemoryTokenRepo();
            await Task.CompletedTask;
            return View("MVCSecret");
        }


        [Authorize]
        public async Task<IActionResult> API1Secret()
        {
            UpdateInMemoryTokenRepo();
            CheckIfRefreshTokenShouldBeUsed().GetAwaiter().GetResult();
            
            HttpResponseMessage httpResponseMessage = await CallURLWithAccessToken("https://localhost:44383/secret", await HttpContext.GetTokenAsync("access_token"));
            var secret = await httpResponseMessage.Content.ReadAsStringAsync();
            return View("API1Secret", new API1SecretViewModel { SecretMessage = secret, httpResponseMessage = httpResponseMessage });
        }

        public async Task<HttpResponseMessage> CallURLWithAccessToken(string url, string accessToken)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.SetBearerToken(accessToken);
            return await httpClient.GetAsync(url);
        }

        private async Task CheckIfRefreshTokenShouldBeUsed()
        {
            if (InMemoryTokenRepo.AccessTokenLifeLeftPercent < 90.0)
            {
                var discoveryDocument = await _httpClientFactory.CreateClient().GetDiscoveryDocumentAsync("https://localhost:44327/");
                var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

                var tokenResponse = await _httpClientFactory.CreateClient().RequestRefreshTokenAsync(
                    new RefreshTokenRequest
                    {
                        Address = discoveryDocument.TokenEndpoint,
                        RefreshToken = refreshToken,
                        ClientId = "client_id_mvc",
                        ClientSecret = "client_secret_mvc"
                    });

                var authInfo = await HttpContext.AuthenticateAsync("mvc_client_cookie");
                authInfo.Properties.UpdateTokenValue("access_token", tokenResponse.AccessToken);
                authInfo.Properties.UpdateTokenValue("id_token", tokenResponse.IdentityToken);
                authInfo.Properties.UpdateTokenValue("refresh_token", tokenResponse.RefreshToken);
                await HttpContext.SignInAsync("mvc_client_cookie", authInfo.Principal, authInfo.Properties);
                UpdateInMemoryTokenRepo();
            }
        }

        public void UpdateInMemoryTokenRepo()
        {
            if (_environment.IsDevelopment())
            {
                var accessToken = HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();
                InMemoryTokenRepo.SetAccessToken(accessToken);
                var idToken = HttpContext.GetTokenAsync("id_token").GetAwaiter().GetResult();
                InMemoryTokenRepo.SetIdToken(idToken);
                var refreshToken = HttpContext.GetTokenAsync("refresh_token").GetAwaiter().GetResult();
                InMemoryTokenRepo.SetRefreshToken(refreshToken);
            }
        }

        public TokenViewModel TokenViewModelFactory()
        {
            TokenViewModel tokenViewModel = new TokenViewModel();
            if (_environment.IsDevelopment())
            {
                var jwtAccessToken = new JwtSecurityTokenHandler().ReadJwtToken(HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult());
                var jwtIdToken = new JwtSecurityTokenHandler().ReadJwtToken(HttpContext.GetTokenAsync("id_token").GetAwaiter().GetResult());

                //Calculate accesstoken lifetime left. The 5 extra minutes is due to the default mintime in is4
                var accessTokenLifetimeLeft = jwtAccessToken.ValidTo.AddMinutes(5) - DateTime.UtcNow;
                var accessTokenTotalLifetime = jwtAccessToken.ValidTo.AddMinutes(5) - jwtAccessToken.ValidFrom;
                InMemoryTokenRepo.AccessTokenLifeLeftPercent = accessTokenLifetimeLeft.TotalSeconds / accessTokenTotalLifetime.TotalSeconds;
                tokenViewModel.AccessTokenLifeLeftPercent = InMemoryTokenRepo.AccessTokenLifeLeftPercent * 100.0;

                //Calculate idtoken lifetime left. The 5 extra minutes is due to the default mintime in is4
                var idTokenLifetimeLeft = jwtIdToken.ValidTo.AddMinutes(5) - DateTime.UtcNow;
                var idTokenTotalLifetime = jwtIdToken.ValidTo.AddMinutes(5) - jwtIdToken.ValidFrom;
                InMemoryTokenRepo.IdTokenLifeLeftPercent = idTokenLifetimeLeft.TotalSeconds / idTokenTotalLifetime.TotalSeconds;
                tokenViewModel.IdTokenLifeLeftPercent = InMemoryTokenRepo.IdTokenLifeLeftPercent * 100.0;

                tokenViewModel.TimeStampLatestUpdate = new DateTime(InMemoryTokenRepo.TimestampLastAccessTokenUpdate).ToString("yyyy-MM-dd HH:mm:ss");

                if (!string.IsNullOrEmpty(InMemoryTokenRepo.AccessToken))
                {
                    //var jwtAccessToken = new JwtSecurityTokenHandler().ReadJwtToken(InMemoryTokenRepo.AccessToken);
                    tokenViewModel.AccessTokenHeader = JValue.Parse(jwtAccessToken.Header.SerializeToJson()).ToString(Formatting.Indented);
                    tokenViewModel.AccessTokenPayload = JValue.Parse(jwtAccessToken.Payload.SerializeToJson()).ToString(Formatting.Indented);
                    tokenViewModel.AccessToken_nbf = jwtAccessToken.ValidFrom.ToString("yyyy-MM-dd HH:mm:ss");
                    tokenViewModel.AccessToken_exp = jwtAccessToken.ValidTo.ToString("yyyy-MM-dd HH:mm:ss");
                    tokenViewModel.AccessToken_auth_time = jwtAccessToken.IssuedAt.ToString("yyyy-MM-dd HH:mm:ss");
                }

                if (!string.IsNullOrEmpty(InMemoryTokenRepo.IdToken))
                {
                    
                    tokenViewModel.IdTokenHeader = JValue.Parse(jwtIdToken.Header.SerializeToJson()).ToString(Formatting.Indented);
                    tokenViewModel.IdTokenPayload = JValue.Parse(jwtIdToken.Payload.SerializeToJson()).ToString(Formatting.Indented);
                    tokenViewModel.IdToken_nbf = jwtIdToken.ValidFrom.ToString("yyyy-MM-dd HH:mm:ss");
                    tokenViewModel.IdToken_exp = jwtIdToken.ValidTo.ToString("yyyy-MM-dd HH:mm:ss");
                    tokenViewModel.IdToken_auth_time = jwtIdToken.IssuedAt.ToString("yyyy-MM-dd HH:mm:ss");
                }
                tokenViewModel.RefreshTokenCode = InMemoryTokenRepo.RefreshToken ?? "";
            }
            return tokenViewModel;
        }
    }
}
