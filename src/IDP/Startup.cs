using IDP.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDP
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

            //Add the toolbox Core Identity. Infrastruktur för Users, password, Claims etc 
            services.AddDbContext<AppDbContext>(config =>
            {
                config.UseInMemoryDatabase("Memory");
            });
            services.AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.SignIn.RequireConfirmedEmail = false;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "IDP.Cookie";
                config.LoginPath = "/Auth/Login";
            });


            //Add the toolbox identityServer4. Infrastruktur för auth, open id connect, clients, apis, scopes
            //Registrerar APIs och Clients, som tillåts accessa denna IDP
            services.AddIdentityServer()
                .AddAspNetIdentity<IdentityUser>()      //Detta limmar ihop is4 med core Identity.
                .AddInMemoryApiResources(Configuration.GetApis())
                .AddInMemoryIdentityResources(Configuration.GetIdentityResources())
                .AddInMemoryClients(Configuration.GetClients())
                .AddDeveloperSigningCredential(); //Genererar certifikat för att signera tokens. Denna ersätter temporärt secretKey jag använde i det rena JWT projektet.

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseIdentityServer(); //IdentityServer4 Nuget

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute(); //Add ... 
            });
        }
    }
}
