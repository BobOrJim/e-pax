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
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private Int64 ThisObjectCreatedTimeStamp;

        public RolesController(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
            ThisObjectCreatedTimeStamp = DateTime.Now.Ticks;
        }


        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole([FromBody] RolesDto rolesViewModel)
        {
            string guid = Guid.NewGuid().ToString();
            ApplicationRole applicationRole = new ApplicationRole
            {
                Id = guid,
                Name = rolesViewModel.NewRoleName.Normalize(),
                NormalizedName = rolesViewModel.NewRoleName.Normalize(),
            };
            var result = await _roleManager.CreateAsync(applicationRole);
            
            return Ok();

        }


        [HttpGet("Roles")]
        //[Authorize(Roles = "Admin, Masters_Degree_In_Mining")] //Access denied
        //[Authorize] //Access denied

        public async Task<IActionResult> Roles()
        {
            IQueryable<ApplicationRole> rolesList = _roleManager.Roles;
            return Ok(rolesList);
        }


        [HttpPost("RemoveRole")]
        public async Task<IActionResult> RemoveRole([FromBody] string Id)
        {
            var d = 1;
            ApplicationRole applicationRoleToRemove = _roleManager.FindByIdAsync(Id).GetAwaiter().GetResult();
            if (_roleManager.RoleExistsAsync(applicationRoleToRemove.Name).GetAwaiter().GetResult())
            {
                _roleManager.DeleteAsync(applicationRoleToRemove).GetAwaiter().GetResult();
            }
            await Task.CompletedTask;
            return Ok();
        }
    }
}
