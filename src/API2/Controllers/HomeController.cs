//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace API2.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Route("/Token")]
        public IActionResult Token()
        {
            //string = InMemoryAccessTokenRepo.TimestampLastAccessTokenUpdate.
            DateTime dt = new DateTime(InMemoryAccessTokenRepo.TimestampLastAccessTokenUpdate);
            string TimestampLastAccessTokenUpdate = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return Ok(new
            {
                InMemoryAccessTokenRepo.AccessToken,
                TimestampLastAccessTokenUpdate,
            });
        }


        [Route("/callapi1secret")] //Detta är start url, dvs detta är här för att spara tid vid testning.
        public async Task<IActionResult> Index()
        {
            //Retrive Access token
            var IDPClient = _httpClientFactory.CreateClient();

            var discoveryDocument = await IDPClient.GetDiscoveryDocumentAsync("https://localhost:44327/");

            //Här specas flow till ClientCredentials, dvs machine to machine
            var TokenResponse = await IDPClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest{
                    Address = discoveryDocument.TokenEndpoint,

                    ClientId = "client_api2",
                    ClientSecret = "client_secret",

                    Scope = "api1",
                });

            //Use token, and get secret data from API1. Dvs vi bygger en http request, med en attatchad bearer token.
            var CallAPI1Client = _httpClientFactory.CreateClient();
            InMemoryAccessTokenRepo.SetAccessToken(TokenResponse.AccessToken);
            CallAPI1Client.SetBearerToken(TokenResponse.AccessToken);
            var SecretResponse = await CallAPI1Client.GetAsync("https://localhost:44383/secret");
            var SecretMessage = await SecretResponse.Content.ReadAsStringAsync();
            //Feltänk av mig nedan, en client har inte olika rättigheter, bara users, duh.
            //var SecretResponse2 = await CallAPI1Client.GetAsync("https://localhost:44383/policy");
            //var SecretMessage2 = await SecretResponse2.Content.ReadAsStringAsync();

            return Ok(SecretMessage);
        }



    }
}
