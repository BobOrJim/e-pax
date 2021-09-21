using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityModel.Client;
using APIGateway1.Services;
using APIGateway1.Constants;
using Microsoft.AspNetCore.Authorization;

namespace APIGateway1.Controllers.V01
{
    [ApiController]
    [Route("api/V01/[controller]")]
    public class ForestController : ControllerBase
    {
        private readonly IForestService _forestService;


        public ForestController(IForestService forestService, IHttpClientFactory httpClientFactory)
        {
            _forestService = forestService;
        }

        [HttpGet("GetSecretForestInEurope")]
        [Authorize]
        public async Task<IActionResult> GetSecretForestInEurope()
        {
            //Retrive Access token
            //var IDPClient = _httpClientFactory.CreateClient();

            //var discoveryDocument = await IDPClient.GetDiscoveryDocumentAsync("https://localhost:44327/");

            ////Här specas flow till ClientCredentials, dvs machine to machine
            //var TokenResponse = await IDPClient.RequestClientCredentialsTokenAsync(
            //    new ClientCredentialsTokenRequest
            //    {
            //        Address = discoveryDocument.TokenEndpoint,
            //        ClientId = "client_apigateway1",
            //        ClientSecret = "apigateway1_secret",
            //        Scope = "API_Forest",
            //    });

            ////Use token, and get secret data from API1. Dvs vi bygger en http request, med en attatchad bearer token.
            //var CallAPI1Client = _httpClientFactory.CreateClient();
            //CallAPI1Client.SetBearerToken(TokenResponse.AccessToken);
            //var SecretResponse = await CallAPI1Client.GetAsync("https://localhost:44380/api/V01/EuropeForests/SecretForestInEurope");
            //var SecretMessage = await SecretResponse.Content.ReadAsStringAsync();

            //return Ok(SecretMessage);
            var SecretMessage = await _forestService.CallEndpoint(API_Forest_Endpoint.SecretForestInEurope);
            return Ok(SecretMessage);
        }
    }
}


