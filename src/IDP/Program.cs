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
            //Syftet med denna uppdeling är att få access till DI services, använda dem, och sedan starta host.
            var host = CreateHostBuilder(args).Build();

            //NOTE: Claims and/or Roles can be used with Core-Identity becouse is4 can work with both.
            //As a personal preference ive decided to use only roles.
            using (var scope = host.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var user = new IdentityUser("bob");
                userManager.CreateAsync(user, "password").GetAwaiter().GetResult();

                userManager.AddClaimAsync(user, new Claim("rc.garndma", "big.cookie")).GetAwaiter().GetResult();
                userManager.AddClaimAsync(user, new Claim("rc.api.garndma", "big.api.cookie")).GetAwaiter().GetResult();
                userManager.AddClaimAsync(user, new Claim("claimname", "claimvalue")).GetAwaiter().GetResult();

                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                //var result = await roleManager.CreateAsync(new IdentityRole("Somevalue"));

                IdentityRole role = new IdentityRole("ASDF");
                roleManager.CreateAsync(role).GetAwaiter().GetResult();
                userManager.AddToRoleAsync(user, "ASDF").GetAwaiter().GetResult();

                IdentityRole role2 = new IdentityRole("ASDF2");
                roleManager.CreateAsync(role2).GetAwaiter().GetResult();
                userManager.AddToRoleAsync(user, "ASDF2").GetAwaiter().GetResult();

                // add the extra role
                //principal.Identities.First().AddClaim(new Claim(ClaimTypes.Role, SomeRole));

                //                modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole { Id = "1", Name = "User", NormalizedName = "USER" });




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
