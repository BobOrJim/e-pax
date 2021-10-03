using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using MVC.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using Microsoft.Extensions.Hosting;
using Common.Extensions;
using Microsoft.AspNetCore.Authentication;
using Common;

namespace MVC.Controllers
{
    [Route("[controller]")]
    public class DetailedUserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DetailedUserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("DetailedUser")]
        public async Task<IActionResult> DetailedUser(string Id)
        {
            DetailedUserViewModel detailedUserViewModel = new DetailedUserViewModel();

            var IDPClient = _httpClientFactory.CreateClient().HttpClientPrep(uri.IDP, await HttpContext.GetTokenAsync("access_token"));

            HttpResponseMessage response = await IDPClient.PostAsJsonAsync("api/V01/DetailedUser/UserNameWithUserId", Id);
            string test = await response.Content.ReadAsStringAsync(); 
            detailedUserViewModel.UserName = test;

            //Call 2 to get allRoles
            response = await IDPClient.GetAsync("api/V01/Roles/Roles");
            List<RoleModel> allRolesRoleModelList = await response.Content.ReadAsAsync<List<RoleModel>>();
            List<string> allRoles = allRolesRoleModelList.Select(r => r.Name).ToList();

            //Call 3 get roles this User have
            response = await IDPClient.PostAsJsonAsync("api/V01/DetailedUser/RolesWithUserId", Id);
            List<string> usersRoles = await response.Content.ReadAsAsync<List<string>>();

            //Render checkboxes with all possible roles. Roles assigned to a user have will have checked checkboxes
            foreach (var role in allRoles) 
            {
                detailedUserViewModel.UsersRoles.Add(new UsersRolesModel { RoleName = role, UserHasThisRole = usersRoles.Contains(role) });
            }
            await Task.CompletedTask;
            return View("DetailedUser", detailedUserViewModel);
        }


        [HttpPost("WriteRolesToUser")]
        public async Task<IActionResult> WriteRolesToUser(DetailedUserViewModel detailedUserViewModel)
        {
            var IDPClient = _httpClientFactory.CreateClient().HttpClientPrep(uri.IDP, await HttpContext.GetTokenAsync("access_token"));
            await IDPClient.PostAsJsonAsync("api/V01/DetailedUser/WriteRolesToUser", detailedUserViewModel);
            return LocalRedirect("/Users/Users");
        }

    }
}

