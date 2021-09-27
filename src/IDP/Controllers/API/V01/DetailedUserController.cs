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
    //[Authorize]
    [Route("api/V01/[controller]")]
    public class DetailedUserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public DetailedUserController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        [HttpPost("UserNameWithUserId")]
        public async Task<IActionResult> UserNameWithUserId([FromBody] string Id)
        {
            string usersName = _userManager.FindByIdAsync(Id).GetAwaiter().GetResult().UserName;
            await Task.CompletedTask;
            return Ok(usersName);
        }


        [HttpPost("RolesWithUserId")]
        public async Task<IActionResult> RolesWithUserId([FromBody] string Id)
        {
            List<string> usersRoles = (List<string>) await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(Id));
            await Task.CompletedTask;
            return Ok(usersRoles);
        }


        [HttpPost("WriteRolesToUser")]
        public async Task<IActionResult> WriteRolesToUser([FromBody] DetailedUserViewModel detailedUserViewModel)
        {

            //Remove all roles from user
            ApplicationUser user = _userManager.FindByNameAsync(detailedUserViewModel.UserName).GetAwaiter().GetResult();
            List<string> rolesToRemove = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().ToList();
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            //Write selected checkboxes roles to user. 
            IEnumerable<UsersRolesModel> selectedUserRolesModels = detailedUserViewModel.UsersRoles.Where(u => u.UserHasThisRole == true);
            IEnumerable<string> selectedRoles = selectedUserRolesModels.Select(r => r.RoleName).ToList();
            var result = await _userManager.AddToRolesAsync(user, selectedRoles);


            await Task.CompletedTask;
            return Ok();
        }
    }
}

