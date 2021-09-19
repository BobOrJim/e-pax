using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;

namespace MVC.Controllers.V1
{
    [Route("V01/[controller]")]
    public class RolesController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHostEnvironment _environment;

        public RolesController(IHttpClientFactory httpClientFactory, IHostEnvironment environment)
        {
            _httpClientFactory = httpClientFactory;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Roles()
        {
            RolesViewModel rolesViewModel = await RolesViewModelFactoryWithRolesLoaded();
            return View("Roles", rolesViewModel);
        }


    }
}
