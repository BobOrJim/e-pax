﻿using IDP.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDP.ViewModels.Auth;
using IdentityServer4.Services;

//Note, antiForgeryTokens are added per default.
namespace IDP.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;

        public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IIdentityServerInteractionService identityServerInteractionService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _identityServerInteractionService = identityServerInteractionService;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View("Login", new LoginRequest { ReturnUrl = returnUrl ?? "https://localhost:44327/" });
        }

        //[HttpGet]
        //public async Task<IActionResult> Logout(string returnUrl, string logoutId)
        //{
        //    await _signInManager.SignOutAsync();

        //    var logoutRequest = await _identityServerInteractionService.GetLogoutContextAsync(logoutId); //Context that do the logout process

        //    var a = 1;

        //    if (string.IsNullOrEmpty(logoutRequest.PostLogoutRedirectUri))
        //    {
        //        //return RedirectToAction("Login", "Auth");
        //        //return RedirectToAction("https://localhost:44345/");
        //        var c = 1;
        //        return Ok();
        //    }

        //    var b = 1;

        //    return Redirect(logoutRequest.PostLogoutRedirectUri);
        //}

        //[HttpGet]
        //public IActionResult Register(string returnUrl)
        //{
        //    return View("Register", new RegisterViewModel { ReturnUrl = returnUrl ?? "https://localhost:44327/" });
        //}

        //[HttpPost]
        //public async Task<IActionResult> Login(LoginRequest vm)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(vm);
        //    }

        //    var result = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, false, false);

        //    if (result.Succeeded)
        //    {
        //        return Redirect(vm.ReturnUrl ?? "https://localhost:44327/");
        //    }
        //    return View("Login", new LoginRequest { ReturnUrl = vm.ReturnUrl ?? "https://localhost:44327/" });
        //}

        //[HttpPost]
        //public async Task<IActionResult> Register(RegisterViewModel vm)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(vm);
        //    }

        //    var user = new ApplicationUser { UserName = vm.Username, NormalizedEmail = vm.Username, EmailConfirmed = true };
        //    var result = await _userManager.CreateAsync(user, vm.Password);

        //    if (result.Succeeded)
        //    {
        //        await _signInManager.SignInAsync(user, false);
        //        return View("Register", new RegisterViewModel { ReturnUrl = vm.ReturnUrl ?? "https://localhost:44327/" });
        //    }

        //    return View("Register", new RegisterViewModel { ReturnUrl = vm.ReturnUrl ?? "https://localhost:44327/" });
        //}
    }
}



