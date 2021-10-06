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
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Common.Extensions;
using Common;

namespace MVC.Controllers
{
    [Route("[controller]")]
    //Note, all Auth are done in the IDP, and no Auth in MVC. This is intentional.
    public class UsersController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public UsersController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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
            var IDPClient = _httpClientFactory.CreateClient().HttpClientPrep(uri.IDP, await HttpContext.GetTokenAsync("access_token"));
            await IDPClient.PostAsJsonAsync("api/V01/Users/RemoveUser", Id);

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

            var IDPClient = _httpClientFactory.CreateClient().HttpClientPrep(uri.IDP, await HttpContext.GetTokenAsync("access_token"));
            //var IDPClient = _httpClientFactory.CreateClient().HttpClientPrep(uri.IDP, await HttpContext.GetTokenAsync("id_token"));

            var responseMessage = await IDPClient.GetAsync("api/V01/Users/Users");

            if (responseMessage.IsSuccessStatusCode)
            {
                try
                {
                    List<UserModel> UsersList = await responseMessage.Content.ReadAsAsync<List<UserModel>>();
                    foreach (var item in UsersList) //Building a userViewModel
                    {
                        UserModel userModel = new UserModel();
                        userModel.Id = item.Id;
                        userModel.normalizedUserName = item.normalizedUserName;
                        usersViewModel.ListOfUsers.Add(userModel);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception in ConsoleUtilities/AskCLIForString. ExceptionType = {e.GetType().FullName} ExceptionMessage = {e.Message}");
                }
            }
            return usersViewModel;
        }
    }
}

