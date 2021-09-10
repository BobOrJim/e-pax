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
    public class DetailedUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private Int64 ThisObjectCreatedTimeStamp;

        public DetailedUserController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            ThisObjectCreatedTimeStamp = DateTime.Now.Ticks;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> DetailedUser(string Id)
        {
            DetailedUserViewModel detailedUserViewModel = new DetailedUserViewModel();
            detailedUserViewModel.UserName = _userManager.FindByIdAsync(Id).GetAwaiter().GetResult().UserName;
            List<string> allRoles = _roleManager.Roles.Select(n => n.Name).ToList();
            List<string> usersRoles = (List<string>) await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(Id));
            foreach (var role in allRoles) //List used to render checkboxes with all possible roles. Roles assigned to a user have will have checked checkboxes
            {
                detailedUserViewModel.UsersRoles.Add(new UsersRolesModel { RoleName = role, UserHasThisRole = usersRoles.Contains(role) });
            }
            await Task.FromResult(0);
            return View("DetailedUser", detailedUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> WriteRolesToUser(DetailedUserViewModel detailedUserViewModel)
        {
            //Clear alla roles for user
            ApplicationUser user = _userManager.FindByNameAsync(detailedUserViewModel.UserName).GetAwaiter().GetResult();
            List<string> allRoles = _roleManager.Roles.Select(e => e.Name).ToList();
            var slask = await _userManager.RemoveFromRolesAsync(user, allRoles);

            int i = 1;

            //Write list of roles fron selected checkboxes
            IEnumerable<UsersRolesModel> selectedUserRolesModels = detailedUserViewModel.UsersRoles.Where(u => u.UserHasThisRole == true);
            List<string> selectedRoles = selectedUserRolesModels.Select(r => r.RoleName).ToList();
            var result = await _userManager.AddToRolesAsync(user, selectedRoles);

            i = 2;

            var test = _userManager.GetRolesAsync(user);

            i = 2;


            await Task.FromResult(0);
            return LocalRedirect("/Users/Users");
        }

    }
}

