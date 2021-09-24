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
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RegisterController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterViewModel vm) //All new Users, get the Userrole by default.
        {
            var user = new ApplicationUser { UserName = vm.Username, NormalizedEmail = vm.Username, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, vm.Password);
            await _userManager.AddToRoleAsync(user, "User"); //New users get "User" role by default
            return Ok();
        }






    }
}

