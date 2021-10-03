using IdentityModel.Client;
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
using MVC.Repos;
using System.Web;
using System.Security.Claims;
using Common;

namespace MVC.Controllers
{
    [Route("[controller]")]
    public class DevController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHostEnvironment _environment;

        public DevController(IHttpClientFactory httpClientFactory, IHostEnvironment environment)
        {
            _httpClientFactory = httpClientFactory;
            _environment = environment;
        }


        [HttpGet("DevPage")]
        public async Task<IActionResult> Dev()
        {
            DevPageViewModel devPageViewModel = new();
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            if (!string.IsNullOrEmpty(accessToken))
            {
                List<Claim> jwtAccessToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken).Claims.Where(a => a.Type == "role").ToList();
                devPageViewModel.UserHasAdminRole = jwtAccessToken.Where(a => a.Value == "Admin").ToList().Any(); //We have a role with value = "Admin"
            }

            await Task.CompletedTask;
            return View("DevPage", devPageViewModel);
        }


        [HttpGet("AskGateway1_GetSecretForestInEurope")]
        [Authorize]
        public async Task<IActionResult> AskGateway1_GetSecretForestInEurope()
        {
            var b = 10;
            SecretMessageViewModel secretMessageViewModel = new();
            secretMessageViewModel.httpResponseMessage = await CallURLWithAccessToken("https://localhost:44370/api/V01/Forest/GetSecretForestInEurope", await HttpContext.GetTokenAsync("access_token"));
            secretMessageViewModel.SecretMessage = await secretMessageViewModel.httpResponseMessage.Content.ReadAsStringAsync();
            return View("SecretMessage", secretMessageViewModel);
        }


        [HttpGet("AskGateway1_GetSecretMountainInEurope")]
        [Authorize]
        public async Task<IActionResult> AskGateway1_GetSecretMountainInEurope()
        {
            SecretMessageViewModel secretMessageViewModel = new();
            secretMessageViewModel.httpResponseMessage = await CallURLWithAccessToken("https://localhost:44370/api/V01/Mountain/GetSecretMountainInEurope", await HttpContext.GetTokenAsync("access_token"));
            secretMessageViewModel.SecretMessage = await secretMessageViewModel.httpResponseMessage.Content.ReadAsStringAsync();
            return View("SecretMessage", secretMessageViewModel);
        }

        [HttpGet("AskGateway2_GetSecretDesertInEurope")]
        [Authorize]
        public async Task<IActionResult> AskGateway1_GetSecretDesertInEurope()
        {
            SecretMessageViewModel secretMessageViewModel = new();
            secretMessageViewModel.httpResponseMessage = await CallURLWithAccessToken("https://localhost:44378/api/V01/Desert/GetSecretDesertInEurope", await HttpContext.GetTokenAsync("access_token"));
            secretMessageViewModel.SecretMessage = await secretMessageViewModel.httpResponseMessage.Content.ReadAsStringAsync();
            return View("SecretMessage", secretMessageViewModel);
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await Task.CompletedTask;
            return SignOut("mvc_client_cookie", "mvc_client_cookieC1", "mvc_client_cookieC2", "IDP_Cookie", "oidc");
        }



        [HttpGet("Token")]
        public async Task<IActionResult> Token()
        {
            //await Task.CompletedTask;
            var token = await TokenViewModelFactory();
            if (token == null)
            {
                return View("Index");
            }
            return View("Token", token);
        }


        [HttpGet("MVCSecret")]
        [Authorize]
        public async Task<IActionResult> MVCSecret()
        {
            await Task.CompletedTask;
            return Ok("This is a secret message, from MVC");
        }


        public async Task<HttpResponseMessage> CallURLWithAccessToken(string url, string accessToken)
        {
            UpdateInMemoryTokenRepo();
            await UseRefreshTokenIfAccessTokenIsOld();
            UpdateInMemoryTokenRepo();
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.SetBearerToken(accessToken);
            return await httpClient.GetAsync(url);
        }


        private async Task UseRefreshTokenIfAccessTokenIsOld()
        {
            if (InMemoryTokenRepo.AccessTokenLifeLeftPercent < 0.85)
            {
                var serverClient = _httpClientFactory.CreateClient();
                var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync(uri.IDP);

                var accessToken = HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();
                var idToken = await HttpContext.GetTokenAsync("id_token");
                var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
                var refreshTokenClient = _httpClientFactory.CreateClient();

                if (!string.IsNullOrEmpty(refreshToken))
                {
                    var tokenResponse = await refreshTokenClient.RequestRefreshTokenAsync(
                    new RefreshTokenRequest
                    {
                        Address = discoveryDocument.TokenEndpoint,
                        RefreshToken = refreshToken,
                        ClientId = "client_mvc",
                        ClientSecret = "client_secret_mvc"
                    });
                    var authInfo = await HttpContext.AuthenticateAsync("mvc_client_cookie");
                    authInfo.Properties.UpdateTokenValue("access_token", tokenResponse.AccessToken);
                    authInfo.Properties.UpdateTokenValue("id_token", tokenResponse.IdentityToken);
                    authInfo.Properties.UpdateTokenValue("refresh_token", tokenResponse.RefreshToken);
                    await HttpContext.SignInAsync("mvc_client_cookie", authInfo.Principal, authInfo.Properties);
                }
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


        public async Task<TokenViewModel> TokenViewModelFactory()
        {
            TokenViewModel tokenViewModel = new TokenViewModel();
            if (_environment.IsDevelopment())
            {

                //Step1: Add access_token to InMemoryTokenRepo & to the TokenViewModel
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                if (!string.IsNullOrEmpty(accessToken))
                {
                    var jwtAccessToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
                    var accessTokenLifetimeLeft = jwtAccessToken.ValidTo.AddMinutes(5) - DateTime.UtcNow; //Calculate accesstoken lifetime left. The 5 extra minutes is due to the default mintime in is4
                    var accessTokenTotalLifetime = jwtAccessToken.ValidTo.AddMinutes(5) - jwtAccessToken.ValidFrom; 
                    InMemoryTokenRepo.AccessTokenLifeLeftPercent = accessTokenLifetimeLeft.TotalSeconds / accessTokenTotalLifetime.TotalSeconds;
                    tokenViewModel.AccessTokenLifeLeftPercent = InMemoryTokenRepo.AccessTokenLifeLeftPercent * 100.0;
                    if (!string.IsNullOrEmpty(InMemoryTokenRepo.AccessToken))
                    {
                        tokenViewModel.AccessTokenHeader = JValue.Parse(jwtAccessToken.Header.SerializeToJson()).ToString(Formatting.Indented);
                        tokenViewModel.AccessTokenPayload = JValue.Parse(jwtAccessToken.Payload.SerializeToJson()).ToString(Formatting.Indented);
                        tokenViewModel.AccessToken_nbf = jwtAccessToken.ValidFrom.ToString("yyyy-MM-dd HH:mm:ss");
                        tokenViewModel.AccessToken_exp = jwtAccessToken.ValidTo.ToString("yyyy-MM-dd HH:mm:ss");
                        tokenViewModel.AccessToken_auth_time = jwtAccessToken.IssuedAt.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }

                //Step2: Add id_token to InMemoryTokenRepo & to the TokenViewModel
                var idToken = await HttpContext.GetTokenAsync("id_token");
                if (!string.IsNullOrEmpty(idToken))
                {
                    var jwtIdToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);
                    //Calculate idtoken lifetime left. The 5 extra minutes is due to the default mintime in is4
                    var idTokenLifetimeLeft = jwtIdToken.ValidTo.AddMinutes(5) - DateTime.UtcNow;
                    var idTokenTotalLifetime = jwtIdToken.ValidTo.AddMinutes(5) - jwtIdToken.ValidFrom;
                    InMemoryTokenRepo.IdTokenLifeLeftPercent = idTokenLifetimeLeft.TotalSeconds / idTokenTotalLifetime.TotalSeconds;
                    tokenViewModel.IdTokenLifeLeftPercent = InMemoryTokenRepo.IdTokenLifeLeftPercent * 100.0;
                    if (!string.IsNullOrEmpty(InMemoryTokenRepo.IdToken))
                    {
                        tokenViewModel.IdTokenHeader = JValue.Parse(jwtIdToken.Header.SerializeToJson()).ToString(Formatting.Indented);
                        tokenViewModel.IdTokenPayload = JValue.Parse(jwtIdToken.Payload.SerializeToJson()).ToString(Formatting.Indented);
                        tokenViewModel.IdToken_nbf = jwtIdToken.ValidFrom.ToString("yyyy-MM-dd HH:mm:ss");
                        tokenViewModel.IdToken_exp = jwtIdToken.ValidTo.ToString("yyyy-MM-dd HH:mm:ss");
                        tokenViewModel.IdToken_auth_time = jwtIdToken.IssuedAt.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }

                //Step3: Add refresh_token to the TokenViewModel
                tokenViewModel.RefreshTokenCode = InMemoryTokenRepo.RefreshToken ?? "";

            }

            //Step4: Move timestamp Latest-update-in-InMemoryTokenRepo to tokenViewModel
            tokenViewModel.TimeStampLatestUpdate = new DateTime(InMemoryTokenRepo.TimestampLastAccessTokenUpdate).ToString("yyyy-MM-dd HH:mm:ss");
            return tokenViewModel;
        }


    } //End of class
} //End of namespace
