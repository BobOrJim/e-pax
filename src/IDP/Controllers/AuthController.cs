using IDP.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDP.ViewModels.Auth;

namespace IDP.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View("Login", new LoginViewModel { ReturnUrl = returnUrl});
        }

        //[IgnoreAntiforgeryToken], per default används antiForgeryToken
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            //Check if the model is valid. and add anti forgery token.

            var result = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, false, false);

            if (result.Succeeded)
            {
                return Redirect(vm.ReturnUrl);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            return View("Register", new RegisterViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            //Check if the model is valid. and add anti forgery token.
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = new ApplicationUser { UserName = vm.Username, NormalizedEmail = vm.Username, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, vm.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Redirect(vm.ReturnUrl);
            }

            return View();
        }
    }
}



