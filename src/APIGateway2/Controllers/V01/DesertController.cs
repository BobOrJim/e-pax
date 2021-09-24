using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityModel.Client;
using APIGateway2.Services;
using APIGateway2.Constants;
using Microsoft.AspNetCore.Authorization;

namespace APIGateway2.Controllers.V01
{
    [ApiController]
    [Route("api/V01/[controller]")]
    public class DesertController : ControllerBase
    {
        private readonly ICallAPIEndpoint _callAPIEndpoint;

        public DesertController(ICallAPIEndpoint callAPIEndpoint)
        {
            _callAPIEndpoint = callAPIEndpoint;
        }

        [HttpGet("GetSecretDesertInEurope")]
        [Authorize(Roles = "Admin, Desert_Master")]
        public async Task<IActionResult> GetSecretDesertInEurope()
        {
            var SecretMessage = await _callAPIEndpoint.CallEndpoint(API_Endpoint.SecretDesertInEurope);
            return Ok(SecretMessage);
        }
    }
}


