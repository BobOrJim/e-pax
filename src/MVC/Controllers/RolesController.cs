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
using MVC.Models;

namespace MVC.Controllers
{
    [Route("[controller]")]
    public class RolesController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHostEnvironment _environment;

        public RolesController(IHttpClientFactory httpClientFactory, IHostEnvironment environment)
        {
            _httpClientFactory = httpClientFactory;
            _environment = environment;
        }


        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole(RolesViewModel rolesViewModel)
        {
            //rolesViewModel.ListOfRoles = JsonConvert.DeserializeObject<List<RoleModel>>(rolesViewModel.jsonSerializeStringPlaceholder1);

            var b = 2;


            var IDPClient = _httpClientFactory.CreateClient();
            IDPClient.BaseAddress = new Uri("https://localhost:44327/");
            HttpResponseMessage response = await IDPClient.PostAsJsonAsync("api/V01/Roles/AddRole", rolesViewModel);

            var a = 12;

            rolesViewModel = await RolesViewModelFactoryWithRolesLoaded();
            return View("Roles", rolesViewModel);

        }


        [HttpGet("Roles")]
        public async Task<IActionResult> Roles()
        {
            var test = 1;
            
            RolesViewModel rolesViewModel = await RolesViewModelFactoryWithRolesLoaded();
            return View("Roles", rolesViewModel);
        }


        [HttpPost("RemoveRole")]
        public async Task<IActionResult> RemoveRole(RolesViewModel rolesViewModel, string Id)
        {
            var IDPClient = _httpClientFactory.CreateClient();
            IDPClient.BaseAddress = new Uri("https://localhost:44327/");
            HttpResponseMessage response = await IDPClient.PostAsJsonAsync("api/V01/Roles/RemoveRole", Id);

            var b = 1;

            await Task.CompletedTask;
            rolesViewModel = await RolesViewModelFactoryWithRolesLoaded();
            return View("Roles", rolesViewModel);
        }


        [HttpPost("Sort")]
        public async Task<IActionResult> Sort(RolesViewModel rolesViewModel)
        {
            rolesViewModel.ListOfRoles = JsonConvert.DeserializeObject<List<RoleModel>>(rolesViewModel.jsonSerializeStringPlaceholder1);
            rolesViewModel.SortAlphabetically = !rolesViewModel.SortAlphabetically;
            if (rolesViewModel.SortAlphabetically)
            {
                rolesViewModel.ListOfRoles = rolesViewModel.ListOfRoles.OrderBy(o => o.Name).ToList();
            }
            else
            {
                rolesViewModel.ListOfRoles = rolesViewModel.ListOfRoles.OrderByDescending(o => o.Name).ToList();
            }
            await Task.CompletedTask;
            return View("Roles", rolesViewModel);
        }


        [HttpPost("SearchFilter")]
        public async Task<IActionResult> SearchFilter(RolesViewModel rolesViewModel)
        {
            string searchPhrase = rolesViewModel.SearchPhrase;
            //rolesViewModel.ListOfRoles = JsonConvert.DeserializeObject<List<RoleModel>>(rolesViewModel.jsonSerializeStringPlaceholder1);
            rolesViewModel = RolesViewModelFactoryWithRolesLoaded().GetAwaiter().GetResult();
            if (String.IsNullOrEmpty(searchPhrase))
            {
                return View("Roles", rolesViewModel);
            }
            rolesViewModel.ListOfRoles = rolesViewModel.ListOfRoles.Where(x => x.Name.Contains(searchPhrase)).ToList();
            await Task.CompletedTask;
            return View("Roles", rolesViewModel);
        }


        public async Task<RolesViewModel> RolesViewModelFactoryWithRolesLoaded() //Good or bad or ugly?
        {
            var a = 1;
            var rolesViewModel = new RolesViewModel();
            var IDPClient = _httpClientFactory.CreateClient();
            IDPClient.BaseAddress = new Uri("https://localhost:44327/");
            //IDPClient.SetBearerToken(TokenResponse.AccessToken);
            HttpResponseMessage SecretResponse = await IDPClient.GetAsync("api/V01/Roles/Roles");
            var SecretMessage = await SecretResponse.Content.ReadAsAsync<List<RoleModel>>();

            //HttpResponseMessage response = await client.PostAsJsonAsync(
            //    "api/products", product);

            var b = 1;

            var rolesList = SecretMessage;
            foreach (var item in rolesList) //gör som anonymt objekt!!, och samma i UserController
            {
                RoleModel roleModel = new RoleModel();
                roleModel.Id = item.Id;
                roleModel.Name = item.Name;
                rolesViewModel.ListOfRoles.Add(roleModel);
            }
            await Task.CompletedTask;
            return rolesViewModel;
        }

    }
}
