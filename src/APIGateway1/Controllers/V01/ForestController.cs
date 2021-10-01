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
        private readonly ICallAPIEndpoint _callAPIEndpoint;
        private readonly IHttpClientFactory _httpClientFactory;

        public ForestController(ICallAPIEndpoint callAPIEndpoint, IHttpClientFactory httpClientFactory)
        {
            _callAPIEndpoint = callAPIEndpoint;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("GetSecretForestInEurope")]
        [Authorize(Roles = "Admin, Masters_Degree_In_Forestry")]
        public async Task<IActionResult> GetSecretForestInEurope()
        {
            //Test: Anslut till IDP via m2m och se om det fungerar.
            //var CallIDPClient = _httpClientFactory.CreateClient();

            //var token = await _tokenFactory.GetAccessToken();
            //CallClient.SetBearerToken(token);
            //var SecretResponse = await CallClient.GetAsync(url);
            //var responseMessage = await SecretResponse.Content.ReadAsStringAsync();
            //return responseMessage;

            var test2 = await _callAPIEndpoint.CallEndpoint("https://localhost:44327/api/V01/Users/Users");


            var CallIDPClient = _httpClientFactory.CreateClient();

            //var token = await _tokenFactory.GetAccessToken();
            //CallClient.SetBearerToken(token);
            //var SecretResponse = await CallClient.GetAsync(url);
            //var responseMessage = await SecretResponse.Content.ReadAsStringAsync();

            var test = await _callAPIEndpoint.CallEndpoint("https://localhost:44327/api/V01/Forests/SecretForestInEurope");

            //var SecretMessage = await _callAPIEndpoint.CallEndpoint(API_Endpoint.SecretForestInEurope); DONT TOUCH

            var SecretMessage = "Haloj";
            var a = 10;

            return Ok(SecretMessage);
        }
    }
}


