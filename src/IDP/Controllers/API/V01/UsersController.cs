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


namespace IDP.Controllers.API.V01
{
    //[Authorize]
    [ApiController]
    
    [Route("api/V01/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        [HttpGet("Users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users()
        {
            IQueryable<ApplicationUser> usersList = _userManager.Users;
            var a = 12;

            return Ok(usersList);
        }


        [HttpPost("RemoveUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveUser([FromBody] string Id)
        {
            ApplicationUser applicationUserToRemove = _userManager.FindByIdAsync(Id).GetAwaiter().GetResult();
            if (_userManager.FindByNameAsync(applicationUserToRemove.UserName).GetAwaiter().GetResult() is not null)
            {
                _userManager.DeleteAsync(applicationUserToRemove).GetAwaiter().GetResult();
            }
            await Task.CompletedTask;
            return Ok();
        }


    }
}

