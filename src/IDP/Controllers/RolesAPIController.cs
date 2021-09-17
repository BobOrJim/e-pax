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
    //[ApiController]
    public class RolesAPIController : ControllerBase
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private Int64 ThisObjectCreatedTimeStamp;

        public RolesAPIController(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
            ThisObjectCreatedTimeStamp = DateTime.Now.Ticks;
        }

        [Route("/RolesAPI/RolesAsync")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> RolesAsync()
        {
            RolesViewModelDto rolesViewModelDto = await RolesViewModelFactoryWithRolesLoaded();
            //return Ok("Hej från RolesAsync");
            return Ok(rolesViewModelDto);
        }

        //[HttpPost]
        //public async Task<IActionResult> AddRole(RolesViewModel rolesViewModel)
        //{
        //    rolesViewModel.ListOfRoles = JsonConvert.DeserializeObject<List<RoleModel>>(rolesViewModel.jsonSerializeStringPlaceholder1);
        //    string guid = Guid.NewGuid().ToString();
        //    ApplicationRole applicationRole = new ApplicationRole
        //    {
        //        Id = guid,
        //        Name = rolesViewModel.NewRoleName.Normalize(),
        //        NormalizedName = rolesViewModel.NewRoleName.Normalize(),
        //    };
        //    var result = await _roleManager.CreateAsync(applicationRole);
        //    rolesViewModel.ListOfRoles.Add(new RoleModel { Id = applicationRole.Id, Name = applicationRole.Name });
        //    if (result.Succeeded) 
        //    {
        //        return View("Roles", rolesViewModel);
        //    }
        //    else
        //    {
        //        return View("Roles", rolesViewModel);
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> RemoveRole(RolesViewModel rolesViewModel, string Id)
        //{
        //    rolesViewModel.ListOfRoles = JsonConvert.DeserializeObject<List<RoleModel>>(rolesViewModel.jsonSerializeStringPlaceholder1);
        //    ApplicationRole applicationRoleToRemove = _roleManager.FindByIdAsync(Id).GetAwaiter().GetResult();
        //    RoleModel roleModelToRemove = rolesViewModel.ListOfRoles.Find(o => o.Id == Id);

        //    if (_roleManager.RoleExistsAsync(applicationRoleToRemove.Name).GetAwaiter().GetResult())
        //    {
        //        _roleManager.DeleteAsync(applicationRoleToRemove).GetAwaiter().GetResult();
        //        rolesViewModel.ListOfRoles.Remove(roleModelToRemove);
        //    }
        //    await Task.CompletedTask;
        //    return View("Roles", rolesViewModel);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Sort(RolesViewModel rolesViewModel)
        //{
        //    rolesViewModel.ListOfRoles = JsonConvert.DeserializeObject<List<RoleModel>>(rolesViewModel.jsonSerializeStringPlaceholder1);
        //    rolesViewModel.SortAlphabetically = !rolesViewModel.SortAlphabetically;
        //    if (rolesViewModel.SortAlphabetically)
        //    {
        //        rolesViewModel.ListOfRoles = rolesViewModel.ListOfRoles.OrderBy(o => o.Name).ToList();
        //    }
        //    else
        //    {
        //        rolesViewModel.ListOfRoles = rolesViewModel.ListOfRoles.OrderByDescending(o => o.Name).ToList();
        //    }
        //    await Task.CompletedTask;
        //    return View("Roles", rolesViewModel);
        //}

        //[HttpPost]
        //public async Task<IActionResult> SearchFilter(RolesViewModel rolesViewModel)
        //{
        //    string searchPhrase = rolesViewModel.SearchPhrase;
        //    //rolesViewModel.ListOfRoles = JsonConvert.DeserializeObject<List<RoleModel>>(rolesViewModel.jsonSerializeStringPlaceholder1);
        //    rolesViewModel = RolesViewModelFactoryWithRolesLoaded().GetAwaiter().GetResult();
        //    if (String.IsNullOrEmpty(searchPhrase))
        //    {
        //        return View("Roles", rolesViewModel);
        //    }
        //    rolesViewModel.ListOfRoles = rolesViewModel.ListOfRoles.Where(x => x.Name.Contains(searchPhrase)).ToList();
        //    await Task.CompletedTask;
        //    return View("Roles", rolesViewModel);
        //}

        public async Task<RolesViewModelDto> RolesViewModelFactoryWithRolesLoaded() //Good or bad or ugly?
        {
            var rolesViewModel = new RolesViewModelDto();
            var rolesList = _roleManager.Roles;
            foreach (var item in rolesList) //CodeSmell use automapper.
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



