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
    public class MountainController : ControllerBase
    {
        private readonly ICallAPIEndpoint _callAPIEndpoint;

        public MountainController(ICallAPIEndpoint callAPIEndpoint)
        {
            _callAPIEndpoint = callAPIEndpoint;
        }

        [HttpGet("GetSecretMountainInEurope")]
        [Authorize(Roles = "Admin, Masters_Degree_In_Mining")]
        public async Task<IActionResult> GetSecretMountainInEurope()
        {
            var SecretMessage = await _callAPIEndpoint.CallEndpoint(API_Endpoint.SecretMountainInEurope);
            return Ok(SecretMessage);
        }
    }
}


