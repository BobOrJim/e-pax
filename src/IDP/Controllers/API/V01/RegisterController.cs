using IDP.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDP.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Diagnostics;
using IDP.ViewModels.Auth;

namespace IDP.Controllers.API.V01
{
    //[Authorize]
    [ApiController]
    [Route("api/V01/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            var b = 1;

            var user = new ApplicationUser { UserName = vm.Username, NormalizedEmail = vm.Username, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, vm.Password);

            var a = 1;

            return Ok();
        }






    }
}

