using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace API_Forest.Controllers.V01
{
    [ApiController]
    [Route("api/V01/[controller]")]
    public class ForestsController : ControllerBase
    {

        [HttpGet("SecretForestInEurope")]
        [Authorize]
        public async Task<IActionResult> SecretForestInEurope()
        {
            await Task.CompletedTask;
            return Ok("Origin = API_Forest. A secret forest in eurpoe is Ardennes");
        }
    }
}



