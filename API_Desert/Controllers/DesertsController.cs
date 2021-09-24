using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Desert.Controllers.V01
{
    [ApiController]
    [Route("api/V01/[controller]")]
    public class DesertsController : ControllerBase
    {

        [HttpGet("SecretDesertInEurope")]
        [Authorize]
        public async Task<IActionResult> SecretDesertInEurope()
        {
            await Task.CompletedTask;
            return Ok("Origin = API_Desert. Settra does not serve, and no secret message you shalt recieve");
        }
    }
}


