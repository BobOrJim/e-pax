using IDP.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IDP.ViewModels.Auth;
using IdentityServer4.Services;
using IDP.Contracts.V01.Requests;

namespace IDP.Controllers.V01
{
    public class AuthController : ControllerBase
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


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(vm);
            //}

            var result = await _signInManager.PasswordSignInAsync(loginRequest.Username, loginRequest.Password, false, false);

            if (result.Succeeded)
            {
                return Ok("result.Succeeded.........hej från Login");
            }
            else
            {
                return Ok("result.Fail.........hej från Login");
            }
        }
    }
}



