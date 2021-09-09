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
    public class RolesController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private Int64 ThisObjectCreatedTimeStamp;

        public RolesController(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
            ThisObjectCreatedTimeStamp = DateTime.Now.Ticks;
        }

        [HttpGet]
        public async Task<IActionResult> Roles()
        {
            RolesViewModel rolesViewModel = await RolesViewModelFactoryWithRolesLoaded();
            return View("Roles", rolesViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(RolesViewModel rolesViewModel)
        {
            rolesViewModel.ListOfRoles = JsonConvert.DeserializeObject<List<RoleModel>>(rolesViewModel.jsonSerializeStringPlaceholder1);
            string guid = Guid.NewGuid().ToString();
            ApplicationRole applicationRole = new ApplicationRole
            {
                Id = guid,
                Name = rolesViewModel.NewRoleName,
                NormalizedName = rolesViewModel.NewRoleName
            };
            var result = await _roleManager.CreateAsync(applicationRole);
            rolesViewModel.ListOfRoles.Add(new RoleModel { Id = applicationRole.Id, Name = applicationRole.Name });
            if (result.Succeeded) 
            {
                return View("Roles", rolesViewModel);
            }
            else
            {
                return View("Roles", rolesViewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(RolesViewModel rolesViewModel, string Id)
        {
            rolesViewModel.ListOfRoles = JsonConvert.DeserializeObject<List<RoleModel>>(rolesViewModel.jsonSerializeStringPlaceholder1);
            ApplicationRole applicationRoleToRemove = _roleManager.FindByIdAsync(Id).GetAwaiter().GetResult();
            RoleModel roleModelToRemove = rolesViewModel.ListOfRoles.Find(o => o.Id == Id);
            bool exists = await _roleManager.RoleExistsAsync(applicationRoleToRemove.Name);
            if (exists)
            {
                _roleManager.DeleteAsync(applicationRoleToRemove).GetAwaiter().GetResult();
                rolesViewModel.ListOfRoles.Remove(roleModelToRemove);
            }
            rolesViewModel.Message = "Hello from RemoveRole-endpoint in RolesController" + "Object created = " + ThisObjectCreatedTimeStamp.ToString();
            return View("Roles", rolesViewModel);
        }

        [HttpPost]
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
            await Task.FromResult(0);
            return View("Roles", rolesViewModel);
        }

        [HttpPost]
        public IActionResult SearchFilter(RolesViewModel rolesViewModel)
        {
            string searchPhrase = rolesViewModel.SearchPhrase;
            //rolesViewModel.ListOfRoles = JsonConvert.DeserializeObject<List<RoleModel>>(rolesViewModel.jsonSerializeStringPlaceholder1);
            rolesViewModel = RolesViewModelFactoryWithRolesLoaded().GetAwaiter().GetResult();
            if (String.IsNullOrEmpty(searchPhrase))
            {
                return View("Roles", rolesViewModel);
            }
            rolesViewModel.ListOfRoles = rolesViewModel.ListOfRoles.Where(x => x.Name.Contains(searchPhrase)).ToList();
            return View("Roles", rolesViewModel);
        }

        public async Task<RolesViewModel> RolesViewModelFactoryWithRolesLoaded() //Good or bad or ugly?
        {
            var rolesViewModel = new RolesViewModel();
            var rolesList = _roleManager.Roles;
            foreach (var item in rolesList) //CodeSmell use automapper.
            {
                RoleModel roleModel = new RoleModel();
                roleModel.Id = item.Id;
                roleModel.Name = item.Name;
                rolesViewModel.ListOfRoles.Add(roleModel);
            }
            await Task.FromResult(0);
            return rolesViewModel;
        }
    }
}



