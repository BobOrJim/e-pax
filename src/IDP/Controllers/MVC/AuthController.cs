using IDP.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IDP.ViewModels.Auth;
using IdentityServer4.Services;
using Serilog;
using Common;

namespace IDP.Controllers.MVC
{
    [Route("[controller]")]
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


        [HttpGet("Login")]
        public IActionResult Login(string returnUrl)
        {
            //Log.Information("Hello, from AuthController.cs in IDP");
            return View("Login", new LoginViewModel { ReturnUrl = returnUrl ?? uri.IDP });
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, false, false);

            if (result.Succeeded)
            {
                return Redirect(vm.ReturnUrl ?? uri.IDP);
            }
            return View("Login", new LoginViewModel { ReturnUrl = vm.ReturnUrl ?? uri.IDP });
        }
    }
}



