using IDP.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDP.ViewModels;
using Microsoft.AspNetCore.Authorization;
using IDP.ViewModels.Roles;
using IDP.Model;
using Newtonsoft.Json;

namespace IDP.Controllers
{
    public class RolesController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private Int64 ThisObjectCreatedTimeStamp;

        public RolesController(SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            ThisObjectCreatedTimeStamp = DateTime.Now.Ticks;
        }

        [HttpGet]
        public async Task<IActionResult> RolesAsync()
        {
            RolesViewModel rolesViewModel = await RolesViewModelFactoryWithRolesLoaded();
            rolesViewModel.Message = "Hello from Roles-endpoint in RolesController" + "Object created = " + ThisObjectCreatedTimeStamp.ToString();
            return View("Roles", rolesViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(RolesViewModel rolesViewModel)
        {
            string guid = Guid.NewGuid().ToString();
            ApplicationRole applicationRole = new ApplicationRole
            {
                Id = guid,
                Name = rolesViewModel.NewRoleName,
                NormalizedName = rolesViewModel.NewRoleName
            };
            var result = await _roleManager.CreateAsync(applicationRole);
            rolesViewModel = await RolesViewModelFactoryWithRolesLoaded();
            if (result.Succeeded) 
            {
                rolesViewModel.Message = "await _roleManager.CreateAsync(applicationRole)  =>  SUCCESS" + "Object created = " + ThisObjectCreatedTimeStamp.ToString();
                return View("Roles", rolesViewModel);
            }
            else
            {
                rolesViewModel.Message = "await _roleManager.CreateAsync(applicationRole)  =>  FAIL" + "Object created = " + ThisObjectCreatedTimeStamp.ToString();
                return View("Roles", rolesViewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(RolesViewModel rolesViewModel, string Id)
        {
            var role = _roleManager.FindByIdAsync(Id).GetAwaiter().GetResult();
            bool exists = await _roleManager.RoleExistsAsync(role.Name);
            if (exists)
            {
                _roleManager.DeleteAsync(role).GetAwaiter().GetResult();
            }
            rolesViewModel = await RolesViewModelFactoryWithRolesLoaded();
            rolesViewModel.Message = "Hello from RemoveRole-endpoint in RolesController" + "Object created = " + ThisObjectCreatedTimeStamp.ToString();
            return View("Roles", rolesViewModel);
        }

        [HttpPost]
        public IActionResult Sort(PeopleViewModel peopleViewModel)
        {
            Debug.WriteLine($" Du tryckte på Sort knappen, på People sidan ");

            //Öppnar. Skriver. Sparar.
            peopleViewModel = JsonConvert.DeserializeObject<PeopleViewModel>(HttpContext.Session.GetString(peopleViewModelKey));
            peopleViewModel.SortAlphabetically = !peopleViewModel.SortAlphabetically;
            if (peopleViewModel.SortAlphabetically)
            {
                peopleViewModel.ListOfPeople = peopleViewModel.ListOfPeople.OrderBy(o => o.Name).ToList();
            }
            else
            {
                peopleViewModel.ListOfPeople = peopleViewModel.ListOfPeople.OrderByDescending(o => o.Name).ToList();
            }
            HttpContext.Session.SetString(peopleViewModelKey, JsonConvert.SerializeObject(peopleViewModel));

            //Debug.WriteLine($"peopleViewModel.SortAlphabetically= {peopleViewModel.SortAlphabetically}");
            //Debug.WriteLine($"peopleViewModel.ListOfPeople.Count= {peopleViewModel.ListOfPeople.Count}");
            return View("People", peopleViewModel);
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
            rolesViewModel.Message = "Hello from SearchFilter-endpoint in RolesController" + "Object created = " + ThisObjectCreatedTimeStamp.ToString();
            return View("Roles", rolesViewModel);
        }

        public async Task<RolesViewModel> RolesViewModelFactoryWithRolesLoaded() //Good or bad or ugly?
        {
            //Populate the list
            var rolesViewModel = new RolesViewModel();
            //IQueryable<ApplicationRole> roles;

            var rolesList = _roleManager.Roles;
            
            //var rolesList = roles.ToList();
            foreach (var item in rolesList) //CodeSmell använd automapper.
            {
                RoleModel roleModel = new RoleModel();
                roleModel.Id = item.Id;
                roleModel.Name = item.Name;
                rolesViewModel.ListOfRoles.Add(roleModel);
            }
            return rolesViewModel;
        }
    }
}



