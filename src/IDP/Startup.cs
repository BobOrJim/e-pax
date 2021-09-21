using IDP.Entities;
using IDP.DBContexts;
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
using IDP.Repos;


using Microsoft.OpenApi.Models;



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
                config.LogoutPath = "/Auth/Logout";
            });


            //Add the toolbox identityServer4. Infrastruktur för auth, open id connect, clients, apis, scopes
            //Registrerar APIs och Clients, som tillåts accessa denna IDP
            services.AddIdentityServer()
                .AddAspNetIdentity<ApplicationUser>()      //Detta limmar ihop is4 med core Identity.
                .AddInMemoryApiResources(IdentityResourcesInMemoryRepo.GetApis())
                .AddInMemoryIdentityResources(IdentityResourcesInMemoryRepo.GetIdentityResources())
                .AddInMemoryClients(IdentityResourcesInMemoryRepo.GetClients())
                .AddDeveloperSigningCredential(); //Genererar certifikat för att signera tokens. Denna ersätter temporärt secretKey jag använde i det rena JWT projektet.

            //services.AddControllersWithViews(); //Skall framöver bli services.AddControllers();

            services.AddControllersWithViews();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IDP", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IDP v1"));
            }

            app.UseStaticFiles(); //To Use bootstrap etc.

            app.UseRouting();

            app.UseAuthorization(); //jim

            app.UseIdentityServer(); //IdentityServer4 Nuget

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute(); //Add ... 
            });
        }
    }
}
