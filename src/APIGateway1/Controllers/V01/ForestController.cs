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
            var SecretMessage = await _callAPIEndpoint.CallEndpoint(API_Endpoint.SecretForestInEurope);
            return Ok(SecretMessage);
        }
    }
}


