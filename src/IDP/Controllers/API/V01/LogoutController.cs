using IDP.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDP.ViewModels;
using Microsoft.AspNetCore.Authorization;
using IDP.Models;
using Newtonsoft.Json;
using System.Diagnostics;

namespace IDP.Controllers.API.V01
{
    [ApiController]
    [Route("api/V01/[controller]")]
    public class LogoutController : ControllerBase
    {



        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await Task.CompletedTask;
            return Redirect("https://localhost:44345/Dev/Devpage");
        }



    }
}

