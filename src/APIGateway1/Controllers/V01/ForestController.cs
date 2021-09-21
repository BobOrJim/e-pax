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
            var SecretMessage = await _forestService.CallEndpoint(API_Forest_Endpoint.SecretForestInEurope);
            return Ok(SecretMessage);
        }
    }
}


