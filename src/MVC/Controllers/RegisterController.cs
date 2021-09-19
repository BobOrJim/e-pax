using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace MVC.Controllers
{
    public class RegisterController : Controller
    {

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            return View("Register", new RegisterViewModel { ReturnUrl = returnUrl ?? "https://localhost:44327/" });
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = new ApplicationUser { UserName = vm.Username, NormalizedEmail = vm.Username, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, vm.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return View("Register", new RegisterViewModel { ReturnUrl = vm.ReturnUrl ?? "https://localhost:44327/" });
            }

            return View("Register", new RegisterViewModel { ReturnUrl = vm.ReturnUrl ?? "https://localhost:44327/" });
        }



    }
}



