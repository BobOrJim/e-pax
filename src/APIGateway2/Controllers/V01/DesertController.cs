using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityModel.Client;
using APIGateway2.Services;
using Microsoft.AspNetCore.Authorization;
using Common.Extensions;
using Microsoft.AspNetCore.Authentication;
using Common;

namespace APIGateway2.Controllers.V01
{
    [ApiController]
    [Route("api/V01/[controller]")]
    public class DesertController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenFactory _tokenFactory;

        public DesertController(IHttpClientFactory httpClientFactory, ITokenFactory tokenFactory)
        {
            _httpClientFactory = httpClientFactory;
            _tokenFactory = tokenFactory;
        }

        [HttpGet("GetSecretDesertInEurope")]
        [Authorize(Roles = "Admin, Desert_Master")]
        public async Task<IActionResult> GetSecretDesertInEurope()
        {
            var API_DesertClient = _httpClientFactory.CreateClient().HttpClientPrep(uri.API_Desert, _tokenFactory.GetAccessToken().GetAwaiter().GetResult());
            var SecretResponse = await API_DesertClient.GetAsync("api/V01/Deserts/SecretDesertInEurope");

            return Ok(await SecretResponse.Content.ReadAsStringAsync());
        }
    }
}


