using IDP.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDP.ViewModels;
using Microsoft.AspNetCore.Authorization;
using IDP.Model;
using Newtonsoft.Json;
using System.Diagnostics;

namespace IDP.Controllers
{
    [ApiController]
    [Route("V01/[controller]")]
    public class RolesAPIController : ControllerBase
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private Int64 ThisObjectCreatedTimeStamp;

        public RolesAPIController(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
            ThisObjectCreatedTimeStamp = DateTime.Now.Ticks;
        }

        [HttpGet]
        public async Task<IActionResult> Roles()
        {
            var rolesList = _roleManager.Roles;
            return Ok(rolesList);
        }

    }
}



