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

namespace MVC.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHostEnvironment _environment;

        public HomeController(IHttpClientFactory httpClientFactory, IHostEnvironment environment)
        {
            _httpClientFactory = httpClientFactory;
            _environment = environment;
        }

        [HttpGet("Dev")]
        public async Task<IActionResult> Dev()
        {

            await Task.CompletedTask;
            return View("Dev");
        }

        [HttpGet("Test")]
        public async Task<IActionResult> Test()
        {
            await Task.CompletedTask;
            return View("Register");
        }




        //public async Task<IActionResult> TestButton()
        //{
        //    await CheckIfRefreshTokenShouldBeUsed();
        //    return View("");
        //}








        //public IActionResult Logout()
        //{
        //    return SignOut("mvc_client_cookie", "oidc"); //
        //}



        //public async Task<IActionResult> Token()
        //{
        //    //await Task.CompletedTask;
        //    var token = await TokenViewModelFactory();
        //    if (token == null)
        //    {
        //        return View("Index");
        //    }

        //    return View("Token", token);

        //}
        //[Authorize]
        //public async Task<IActionResult> MVCSecret()
        //{
        //    //UpdateInMemoryTokenRepo();
        //    //await CheckIfRefreshTokenShouldBeUsed();
        //    await Task.CompletedTask;
        //    return View("MVCSecret");
        //}
        //[Authorize]
        //public async Task<IActionResult> API1Secret()
        //{
        //    var a = 1;

        //    HttpResponseMessage httpResponseMessage = await CallURLWithAccessToken("https://localhost:44383/secret", await HttpContext.GetTokenAsync("access_token"));
        //    var secret = await httpResponseMessage.Content.ReadAsStringAsync();
        //    return View("API1Secret", new API1SecretViewModel { SecretMessage = secret, httpResponseMessage = httpResponseMessage });
        //}

        //public async Task<HttpResponseMessage> CallURLWithAccessToken(string url, string accessToken)
        //{
        //    UpdateInMemoryTokenRepo();
        //    await CheckIfRefreshTokenShouldBeUsed();
        //    UpdateInMemoryTokenRepo();
        //    var httpClient = _httpClientFactory.CreateClient();
        //    httpClient.SetBearerToken(accessToken);
        //    return await httpClient.GetAsync(url);
        //}

        //private async Task RefreshAccessToken()
        //{
        //    var serverClient = _httpClientFactory.CreateClient();
        //    var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:44305/");

        //    var accessToken = await HttpContext.GetTokenAsync("access_token");
        //    var idToken = await HttpContext.GetTokenAsync("id_token");
        //    var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
        //    var refreshTokenClient = _httpClientFactory.CreateClient();

        //    var tokenResponse = await refreshTokenClient.RequestRefreshTokenAsync(
        //        new RefreshTokenRequest
        //        {
        //            Address = discoveryDocument.TokenEndpoint,
        //            RefreshToken = refreshToken,
        //            ClientId = "client_id_mvc",
        //            ClientSecret = "client_secret_mvc"
        //        });

        //    var authInfo = await HttpContext.AuthenticateAsync("Cookie");

        //    authInfo.Properties.UpdateTokenValue("access_token", tokenResponse.AccessToken);
        //    authInfo.Properties.UpdateTokenValue("id_token", tokenResponse.IdentityToken);
        //    authInfo.Properties.UpdateTokenValue("refresh_token", tokenResponse.RefreshToken);

        //    await HttpContext.SignInAsync("Cookie", authInfo.Principal, authInfo.Properties);
        //}

        //private async Task CheckIfRefreshTokenShouldBeUsed()
        //{



        //    if (InMemoryTokenRepo.AccessTokenLifeLeftPercent < 0.85) //if (InMemoryTokenRepo.AccessTokenLifeLeftPercent < 90.0)
        //    {
        //        //InMemoryTokenRepo.TimestampLastAccessTokenUpdate = 10
        //        var a = 1;

        //        var serverClient = _httpClientFactory.CreateClient();
        //        var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:44327/");

        //        var accessToken = HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();
        //        var idToken = await HttpContext.GetTokenAsync("id_token");
        //        var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
        //        var refreshTokenClient = _httpClientFactory.CreateClient();
        //        //var discoveryDocument = await _httpClientFactory.CreateClient().GetDiscoveryDocumentAsync("https://localhost:44327/");
        //        //var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

        //        var c = 1;

        //        if (!string.IsNullOrEmpty(refreshToken))
        //        {
        //            var tokenResponse = await refreshTokenClient.RequestRefreshTokenAsync(
        //            new RefreshTokenRequest
        //            {
        //                Address = discoveryDocument.TokenEndpoint,
        //                RefreshToken = refreshToken,
        //                ClientId = "client_mvc",
        //                ClientSecret = "client_secret_mvc"
        //            });


        //            var b = 1;
        //            var authInfo = await HttpContext.AuthenticateAsync("mvc_client_cookie");
        //            authInfo.Properties.UpdateTokenValue("access_token", tokenResponse.AccessToken);
        //            authInfo.Properties.UpdateTokenValue("id_token", tokenResponse.IdentityToken);
        //            authInfo.Properties.UpdateTokenValue("refresh_token", tokenResponse.RefreshToken);
        //            await HttpContext.SignInAsync("mvc_client_cookie", authInfo.Principal, authInfo.Properties);
        //        }


        //        //UpdateInMemoryTokenRepo();
        //    }
        //}

        //public void UpdateInMemoryTokenRepo()
        //{
        //    if (_environment.IsDevelopment())
        //    {
        //        var accessToken = HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();
        //        InMemoryTokenRepo.SetAccessToken(accessToken);
        //        var idToken = HttpContext.GetTokenAsync("id_token").GetAwaiter().GetResult();
        //        InMemoryTokenRepo.SetIdToken(idToken);
        //        var refreshToken = HttpContext.GetTokenAsync("refresh_token").GetAwaiter().GetResult();
        //        InMemoryTokenRepo.SetRefreshToken(refreshToken);
        //    }
        //}

        //public async Task<TokenViewModel> TokenViewModelFactory()
        //{
        //    TokenViewModel tokenViewModel = new TokenViewModel();
        //    if (_environment.IsDevelopment())
        //    {
        //        //System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.ReadJwtToken(string token)

        //        var accessToken = await HttpContext.GetTokenAsync("access_token");
        //        if (!string.IsNullOrEmpty(accessToken))
        //        {
        //            var jwtAccessToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
        //            //Calculate accesstoken lifetime left. The 5 extra minutes is due to the default mintime in is4
        //            var accessTokenLifetimeLeft = jwtAccessToken.ValidTo.AddMinutes(5) - DateTime.UtcNow;
        //            var accessTokenTotalLifetime = jwtAccessToken.ValidTo.AddMinutes(5) - jwtAccessToken.ValidFrom;
        //            InMemoryTokenRepo.AccessTokenLifeLeftPercent = accessTokenLifetimeLeft.TotalSeconds / accessTokenTotalLifetime.TotalSeconds;
        //            tokenViewModel.AccessTokenLifeLeftPercent = InMemoryTokenRepo.AccessTokenLifeLeftPercent * 100.0;
        //            if (!string.IsNullOrEmpty(InMemoryTokenRepo.AccessToken))
        //            {
        //                //var jwtAccessToken = new JwtSecurityTokenHandler().ReadJwtToken(InMemoryTokenRepo.AccessToken);
        //                tokenViewModel.AccessTokenHeader = JValue.Parse(jwtAccessToken.Header.SerializeToJson()).ToString(Formatting.Indented);
        //                tokenViewModel.AccessTokenPayload = JValue.Parse(jwtAccessToken.Payload.SerializeToJson()).ToString(Formatting.Indented);
        //                tokenViewModel.AccessToken_nbf = jwtAccessToken.ValidFrom.ToString("yyyy-MM-dd HH:mm:ss");
        //                tokenViewModel.AccessToken_exp = jwtAccessToken.ValidTo.ToString("yyyy-MM-dd HH:mm:ss");
        //                tokenViewModel.AccessToken_auth_time = jwtAccessToken.IssuedAt.ToString("yyyy-MM-dd HH:mm:ss");
        //            }
        //        }


        //        var idToken = await HttpContext.GetTokenAsync("id_token");
        //        if (!string.IsNullOrEmpty(idToken))
        //        {
        //            var jwtIdToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);
        //            //Calculate idtoken lifetime left. The 5 extra minutes is due to the default mintime in is4
        //            var idTokenLifetimeLeft = jwtIdToken.ValidTo.AddMinutes(5) - DateTime.UtcNow;
        //            var idTokenTotalLifetime = jwtIdToken.ValidTo.AddMinutes(5) - jwtIdToken.ValidFrom;
        //            InMemoryTokenRepo.IdTokenLifeLeftPercent = idTokenLifetimeLeft.TotalSeconds / idTokenTotalLifetime.TotalSeconds;
        //            tokenViewModel.IdTokenLifeLeftPercent = InMemoryTokenRepo.IdTokenLifeLeftPercent * 100.0;
        //            if (!string.IsNullOrEmpty(InMemoryTokenRepo.IdToken))
        //            {
        //                tokenViewModel.IdTokenHeader = JValue.Parse(jwtIdToken.Header.SerializeToJson()).ToString(Formatting.Indented);
        //                tokenViewModel.IdTokenPayload = JValue.Parse(jwtIdToken.Payload.SerializeToJson()).ToString(Formatting.Indented);
        //                tokenViewModel.IdToken_nbf = jwtIdToken.ValidFrom.ToString("yyyy-MM-dd HH:mm:ss");
        //                tokenViewModel.IdToken_exp = jwtIdToken.ValidTo.ToString("yyyy-MM-dd HH:mm:ss");
        //                tokenViewModel.IdToken_auth_time = jwtIdToken.IssuedAt.ToString("yyyy-MM-dd HH:mm:ss");
        //            }
        //        }







        //        tokenViewModel.RefreshTokenCode = InMemoryTokenRepo.RefreshToken ?? "";
        //    }
        //    tokenViewModel.TimeStampLatestUpdate = new DateTime(InMemoryTokenRepo.TimestampLastAccessTokenUpdate).ToString("yyyy-MM-dd HH:mm:ss");
        //    return tokenViewModel;
        //}
    }
}
