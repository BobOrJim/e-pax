using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
using System.Diagnostics;
using System.Text.Json;

namespace MVC.Controllers
{
    [Route("[controller]")]
    //[Authorize]
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


        public async Task<UsersViewModel> UsersViewModelFactoryWithUsersLoaded() //Good or bad or ugly?
        {
            var usersViewModel = new UsersViewModel();
            var IDPClient = _httpClientFactory.CreateClient();
            IDPClient.BaseAddress = new Uri("https://localhost:44327/");
            //IDPClient.SetBearerToken(TokenResponse.AccessToken);
            HttpResponseMessage SecretResponse = await IDPClient.GetAsync("api/V01/Users/Users");
            List<UserModel> UsersList2 = await SecretResponse.Content.ReadAsAsync<List<UserModel>>(); //OBS, här sker en "okontrollerad" default mappning. FIXA med automapper på alla ställen
            
            
            //Kanske bra att ha när jag ger mig på automapper
            //var UsersList = await SecretResponse.Content.ReadAsStringAsync();
            //var response = await IDPClient.GetAsync($"api/V01/Users/Users");
            //if (!response.IsSuccessStatusCode)
            //{
            //    Debug.WriteLine($" {response.ReasonPhrase}");
            //}
            //var dataAsString = await response.Content.ReadAsStringAsync();



            var a = 10;

            foreach (var item in UsersList2)
            {
                UserModel userModel = new UserModel();
                userModel.Id = item.Id;
                userModel.normalizedUserName = item.normalizedUserName;
                usersViewModel.ListOfUsers.Add(userModel);
            }
            return usersViewModel;
        }


    }
}

