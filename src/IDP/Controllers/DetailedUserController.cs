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
            //Remove all roles from user
            ApplicationUser user = _userManager.FindByNameAsync(detailedUserViewModel.UserName).GetAwaiter().GetResult();
            List<string> rolesToRemove = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().ToList();
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            //Write selected checkboxes roles to user. 
            IEnumerable<UsersRolesModel> selectedUserRolesModels = detailedUserViewModel.UsersRoles.Where(u => u.UserHasThisRole == true);
            IEnumerable<string> selectedRoles = selectedUserRolesModels.Select(r => r.RoleName).ToList();
            var result = await _userManager.AddToRolesAsync(user, selectedRoles);




            await Task.FromResult(0);
            return LocalRedirect("/Users/Users");
        }

    }
}

