using IDP.Entities;
using IDP.DBContexts;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IDP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Hi. First thing first weary traveler and friend. I hope you drop me an email so i can buy you a cold beer in Borås. Jimmy.Nordin.1979@gmail.com

            //To get access to DI services before the host is started.
            var host = CreateHostBuilder(args).Build();


            //NOTE: Claims and/or Roles can be used with Core-Identity becouse is4 can work with both.
            //As a personal preference ive decided to use only roles.
            using (var scope = host.Services.CreateScope())
            {
                //var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                //var user = new ApplicationUser { UserName = "bob", NormalizedEmail = "bob", EmailConfirmed = true };
                ////var user = new ApplicationUser("bob");
                //userManager.CreateAsync(user, "password").GetAwaiter().GetResult();

                //userManager.AddClaimAsync(user, new Claim("rc.garndma", "big.cookie")).GetAwaiter().GetResult();
                //userManager.AddClaimAsync(user, new Claim("rc.api.garndma", "big.api.cookie")).GetAwaiter().GetResult();
                //userManager.AddClaimAsync(user, new Claim("claimname", "claimvalue")).GetAwaiter().GetResult();

                //var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();


                //Seeding some more stuff
                //ApplicationRole role = new ApplicationRole { Id = "20", Name = "ASDF", NormalizedName = "ASDF", ConcurrencyStamp = "cf1ac99a-9b57-44d7-9e0f-a33f3bf61a47"};
                //roleManager.CreateAsync(role).GetAwaiter().GetResult();
                //userManager.AddToRoleAsync(user, "ASDF").GetAwaiter().GetResult();

                //ApplicationRole role2 = new ApplicationRole { Id = "21", Name = "ASDF2", NormalizedName = "ASDF2" };
                //roleManager.CreateAsync(role2).GetAwaiter().GetResult();
                //userManager.AddToRoleAsync(user, "ASDF2").GetAwaiter().GetResult();





                //userManager.AddClaimAsync(user, new Claim("asdf", "qwerty")).GetAwaiter().GetResult();


                ////Denna är adderad till identity token
                //userManager.AddClaimAsync(user,
                //    new Claim("rc.garndma", "big.cookie"))
                //    .GetAwaiter().GetResult();

                ////Denna vill vi lägga till accessToken, för då kan vi senare konvertera en claim till en policy. och en policy kan vi dekorera endpoints med.
                //userManager.AddClaimAsync(user,
                //    new Claim("rc.api.garndma", "big.api.cookie"))
                //    .GetAwaiter().GetResult();

                //userManager.AddClaimAsync(user,
                //    new Claim("Claim", "apelsin")).GetAwaiter().GetResult();


            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
