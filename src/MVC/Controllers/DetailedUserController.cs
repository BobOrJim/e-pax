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

namespace MVC.Controllers
{
    public class DetailedUserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHostEnvironment _environment;

        public DetailedUserController(IHttpClientFactory httpClientFactory, IHostEnvironment environment)
        {
            _httpClientFactory = httpClientFactory;
            _environment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> DetailedUser(string Id)
        {
            DetailedUserViewModel detailedUserViewModel = new DetailedUserViewModel();

            //Call 1 to get UserName from Id
            var IDPClient = _httpClientFactory.CreateClient();
            IDPClient.BaseAddress = new Uri("https://localhost:44327/");
            //IDPClient.SetBearerToken(TokenResponse.AccessToken);
            HttpResponseMessage response = await IDPClient.PostAsJsonAsync("api/V01/DetailedUser/UserNameWithUserId", Id);
            string test = await response.Content.ReadAsStringAsync(); //ful mappad...fixa senare...
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


        [HttpPost]
        public async Task<IActionResult> WriteRolesToUser(DetailedUserViewModel detailedUserViewModel)
        {
            var IDPClient = _httpClientFactory.CreateClient();
            IDPClient.BaseAddress = new Uri("https://localhost:44327/");
            //IDPClient.SetBearerToken(TokenResponse.AccessToken);
            await IDPClient.PostAsJsonAsync("api/V01/DetailedUser/WriteRolesToUser", detailedUserViewModel);
            return LocalRedirect("/Users/Users");
        }

    }
}

