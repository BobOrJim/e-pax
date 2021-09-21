using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityModel.Client;

namespace APIGateway1.Controllers.V01
{
    [ApiController]
    [Route("api/V01/[controller]")]
    public class ForestController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ForestController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> GetSecretForestInEurope()
        {
            //Retrive Access token
            var IDPClient = _httpClientFactory.CreateClient();
            var discoveryDocument = await IDPClient.GetDiscoveryDocumentAsync("https://localhost:44327/");

            //Här specas flow till ClientCredentials, dvs machine to machine
            var TokenResponse = await IDPClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,
                    ClientId = "APIGateway1",
                    ClientSecret = "APIGateway1_secret",
                    Scope = "API_Forest",
                });

            //Use token, and get secret data from API1. Dvs vi bygger en http request, med en attatchad bearer token.
            var CallAPI1Client = _httpClientFactory.CreateClient();
            //InMemoryAccessTokenRepo.SetAccessToken(TokenResponse.AccessToken);
            CallAPI1Client.SetBearerToken(TokenResponse.AccessToken);
            var SecretResponse = await CallAPI1Client.GetAsync("https://localhost:44380/api/V01/EuropeForests/SecretForestInEurope");
            var SecretMessage = await SecretResponse.Content.ReadAsStringAsync();



            var a = 1;

            //Här skall det ske ett anrop till ForestService, via ett interface, där API3Service använder en httpClient
            //Och anropar mikrotjänst API3.
            //return Ok();
            
            
            await Task.CompletedTask;
            return Ok("Temporary secret message from API_Forest, secret forest in Europe is Ardennes");
        }
    }
}
