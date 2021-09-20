using IDP.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IDP.ViewModels.Auth;
using IdentityServer4.Services;


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
            return View("Login", new LoginViewModel { ReturnUrl = returnUrl ?? "https://localhost:44327/" });
        }


        //[IgnoreAntiforgeryToken], per default används antiForgeryToken
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
                return Redirect(vm.ReturnUrl ?? "https://localhost:44327/");
            }
            return View("Login", new LoginViewModel { ReturnUrl = vm.ReturnUrl ?? "https://localhost:44327/" });
        }



    }
}



