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
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private Int64 ThisObjectCreatedTimeStamp;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            ThisObjectCreatedTimeStamp = DateTime.Now.Ticks;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            UsersViewModel usersViewModel = await UsersViewModelFactoryWithUsersLoaded();
            return View("Users", usersViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UsersViewModel usersViewModel)
        {
            usersViewModel.ListOfUsers = JsonConvert.DeserializeObject<List<UserModel>>(usersViewModel.jsonSerializeStringPlaceholder1);
            string guid = Guid.NewGuid().ToString();
            ApplicationUser applicationUser = new ApplicationUser
            {
                Id = guid,
                UserName = usersViewModel.NewUserName.Normalize(),
                NormalizedUserName = usersViewModel.NewUserName.Normalize(),
            };
            var result = await _userManager.CreateAsync(applicationUser);
            usersViewModel.ListOfUsers.Add(new UserModel { Id = applicationUser.Id, Name = applicationUser.UserName });
            if (result.Succeeded)
            {
                return View("Users", usersViewModel);
            }
            else
            {
                return View("Users", usersViewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUser(UsersViewModel usersViewModel, string Id)
        {
            usersViewModel.ListOfUsers = JsonConvert.DeserializeObject<List<UserModel>>(usersViewModel.jsonSerializeStringPlaceholder1);
            ApplicationUser applicationUserToRemove = _userManager.FindByIdAsync(Id).GetAwaiter().GetResult();
            UserModel userModelToRemove = usersViewModel.ListOfUsers.Find(o => o.Id == Id);
            _userManager.FindByNameAsync(applicationUserToRemove.UserName).GetAwaiter().GetResult();
            if (_userManager.FindByNameAsync(applicationUserToRemove.UserName).GetAwaiter().GetResult() is not null)
            {
                _userManager.DeleteAsync(applicationUserToRemove).GetAwaiter().GetResult();
                usersViewModel.ListOfUsers.Remove(userModelToRemove);
            }
            usersViewModel.Message = "Hello from RemoveRole-endpoint in RolesController" + "Object created = " + ThisObjectCreatedTimeStamp.ToString();
            await Task.FromResult(0);
            return View("Users", usersViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Sort(UsersViewModel usersViewModel)
        {
            usersViewModel.ListOfUsers = JsonConvert.DeserializeObject<List<UserModel>>(usersViewModel.jsonSerializeStringPlaceholder1);
            usersViewModel.SortAlphabetically = !usersViewModel.SortAlphabetically;
            if (usersViewModel.SortAlphabetically)
            {
                usersViewModel.ListOfUsers = usersViewModel.ListOfUsers.OrderBy(o => o.Name).ToList();
            }
            else
            {
                usersViewModel.ListOfUsers = usersViewModel.ListOfUsers.OrderByDescending(o => o.Name).ToList();
            }
            await Task.FromResult(0);
            return View("Users", usersViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SearchFilter(UsersViewModel usersViewModel)
        {
            string searchPhrase = usersViewModel.SearchPhrase;
            //rolesViewModel.ListOfRoles = JsonConvert.DeserializeObject<List<RoleModel>>(rolesViewModel.jsonSerializeStringPlaceholder1);
            usersViewModel = UsersViewModelFactoryWithUsersLoaded().GetAwaiter().GetResult();
            if (String.IsNullOrEmpty(searchPhrase))
            {
                return View("Users", usersViewModel);
            }
            usersViewModel.ListOfUsers = usersViewModel.ListOfUsers.Where(x => x.Name.Contains(searchPhrase)).ToList();
            await Task.FromResult(0);
            return View("Users", usersViewModel);
        }

        public async Task<UsersViewModel> UsersViewModelFactoryWithUsersLoaded() //Good or bad or ugly?
        {
            var usersViewModel = new UsersViewModel();
            var usersList = _userManager.Users;
            foreach (var item in usersList) //CodeSmell use automapper.
            {
                UserModel userModel = new UserModel();
                userModel.Id = item.Id;
                userModel.Name = item.UserName;
                usersViewModel.ListOfUsers.Add(userModel);
            }
            await Task.FromResult(0);
            return usersViewModel;
        }
    }
}

