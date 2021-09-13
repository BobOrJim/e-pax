using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace API1.Controllers
{
    public class SecretController : ControllerBase //Note Controller have support for views, vi kör api och använder då endast ControllerBase
    {

        [Route("/notsecret")]
        //[Authorize]
        public IActionResult notsecret()
        {
            return Ok("not secret message from api1");
        }

        [Route("/secret")]
        [Authorize]
        public string Index()
        {
            //var claims = User.Claims.ToList();
            return "secret message from api1";
        }

        [Route("/policy")]
        //[Authorize(Policy = "EmployeeOnly")]
        //[Authorize(AuthenticationSchemes = "")]
        public string Test1()
        {
            //var claims = User.Claims.ToList();
            return "secret policy message from api1";
        }

        [Route("/rolepolicy")]
        [Authorize(Roles = "ASDF")]
        public string Test2()
        {
            return "secret rolepolicy message from api1";
        }
    }
}
