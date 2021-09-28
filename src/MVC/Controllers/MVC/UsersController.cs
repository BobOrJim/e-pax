﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using MVC.Models;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using Newtonsoft.Json;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using MVC.Extensions;

namespace MVC.Controllers
{
    [Route("[controller]")]
    //Note, all Auth are done in the IDP, and no Auth in MVC. This is intentional.
    public class UsersController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHostEnvironment _environment;

        public UsersController(IHttpClientFactory httpClientFactory, IHostEnvironment environment)
        {
            _httpClientFactory = httpClientFactory;
            _environment = environment;
        }


        [HttpGet("Users")]
        public async Task<IActionResult> Users()
        {
            UsersViewModel usersViewModel = await UsersViewModelFactoryWithUsersLoaded();
            return View("Users", usersViewModel);
        }


        [HttpPost("RemoveUser")]
        public async Task<IActionResult> RemoveUser(UsersViewModel usersViewModel, string Id)
        {
            var IDPClient = _httpClientFactory.CreateClient();
            IDPClient.BaseAddress = new Uri("https://localhost:44327/");
            HttpResponseMessage response = await IDPClient.PostAsJsonAsync("api/V01/Users/RemoveUser", Id);

            usersViewModel = await UsersViewModelFactoryWithUsersLoaded();
            return View("Users", usersViewModel);
        }


        [HttpPost("Sort")]
        public async Task<IActionResult> Sort(UsersViewModel usersViewModel)
        {
            usersViewModel.ListOfUsers = JsonConvert.DeserializeObject<List<UserModel>>(usersViewModel.jsonSerializeStringPlaceholder1);
            usersViewModel.SortAlphabetically = !usersViewModel.SortAlphabetically;
            if (usersViewModel.SortAlphabetically)
            {
                usersViewModel.ListOfUsers = usersViewModel.ListOfUsers.OrderBy(o => o.normalizedUserName).ToList();
            }
            else
            {
                usersViewModel.ListOfUsers = usersViewModel.ListOfUsers.OrderByDescending(o => o.normalizedUserName).ToList();
            }
            await Task.CompletedTask;
            return View("Users", usersViewModel);
        }


        [HttpPost("SearchFilter")]
        public async Task<IActionResult> SearchFilter(UsersViewModel usersViewModel)
        {
            string searchPhrase = usersViewModel.SearchPhrase;
            usersViewModel = UsersViewModelFactoryWithUsersLoaded().GetAwaiter().GetResult();
            if (String.IsNullOrEmpty(searchPhrase))
            {
                return View("Users", usersViewModel);
            }
            usersViewModel.ListOfUsers = usersViewModel.ListOfUsers.Where(x => x.normalizedUserName.Contains(searchPhrase)).ToList();
            await Task.CompletedTask;
            return View("Users", usersViewModel);
        }


        public async Task<UsersViewModel> UsersViewModelFactoryWithUsersLoaded()
        {
            var usersViewModel = new UsersViewModel();
            //var IDPClient = _httpClientFactory.CreateClient().HttpClientPrep("https://localhost:44327/", await HttpContext.GetTokenAsync("access_token"));
            var IDPClient = _httpClientFactory.CreateClient();

            IDPClient.BaseAddress = new Uri("https://localhost:44327/");
            var token = await HttpContext.GetTokenAsync("access_token");
            IDPClient.SetBearerToken(token);

            HttpResponseMessage responseMessage = await IDPClient.GetAsync("api/V01/Users/Users");


            //var IDPClient = _httpClientFactory.CreateClient();
            //IDPClient.BaseAddress = new Uri("https://localhost:44327/");
            //IDPClient.SetBearerToken(await HttpContext.GetTokenAsync("access_token"));

            //HttpResponseMessage SecretResponse = await IDPClient.GetAsync("api/V01/Roles/Roles");
            //var rolesList = await SecretResponse.Content.ReadAsAsync<List<RoleModel>>();


            var a = 12;

            if (responseMessage.IsSuccessStatusCode)
            {
                try
                {
                    var b = 12;
                    List<UserModel> UsersList = await responseMessage.Content.ReadAsAsync<List<UserModel>>();
                    var live = 12;
                    foreach (var item in UsersList) //Building a userViewModel
                    {
                        UserModel userModel = new UserModel();
                        userModel.Id = item.Id;
                        userModel.normalizedUserName = item.normalizedUserName;
                        usersViewModel.ListOfUsers.Add(userModel);
                    }
                    var dim = 12;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception in ConsoleUtilities/AskCLIForString. ExceptionType = {e.GetType().FullName} ExceptionMessage = {e.Message}");
                }
            }
            var c = 12;

            return usersViewModel;
        }
    }
}

