using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Mountain.Controllers.V01
{
    [ApiController]
    [Route("api/V01/[controller]")]
    public class MountainsController : ControllerBase
    {

        [HttpGet("SecretMountainInEurope")]
        [Authorize]
        public async Task<IActionResult> SecretMountainInEurope()
        {
            await Task.CompletedTask;
            return Ok("Origin = API_Mountain. A secret mountain in eurpoe is the alps.");
        }
    }
}



