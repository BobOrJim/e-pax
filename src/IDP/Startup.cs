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
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;

namespace IDP
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {

            // Add EF services to the services container.
            services.AddDbContext<ApplicationIdentityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IDPContextConnection")));




            //Add the toolbox Core Identity. Infrastruktur för Users, password, Claims etc 
            //services.AddDbContext<AppDbContext>(config =>
            //{
            //    config.UseInMemoryDatabase("Memory");
            //});

            services.AddIdentity<ApplicationUser, ApplicationRole>(config =>
            {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.SignIn.RequireConfirmedEmail = false;
            })
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();
            
            //Cookie settings
            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "IDP.Cookie";
                config.LoginPath = "/Auth/Login";
            });


            //Add the toolbox identityServer4. Infrastruktur för auth, open id connect, clients, apis, scopes
            //Registrerar APIs och Clients, som tillåts accessa denna IDP
            services.AddIdentityServer()
                .AddAspNetIdentity<ApplicationUser>()      //Detta limmar ihop is4 med core Identity.
                .AddInMemoryApiResources(MyConfiguration.GetApis())
                .AddInMemoryIdentityResources(MyConfiguration.GetIdentityResources())
                .AddInMemoryClients(MyConfiguration.GetClients())
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
