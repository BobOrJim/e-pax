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
using Serilog;
using Common;
using Microsoft.IdentityModel.Tokens;

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
            //############## TRYING TO GET IDP TO REGISTER ITSELF AT A API RESORCE IN ITSELF.
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("role", "Admin");
                });
            });
            //#############



            // Add EF services to the services container. IE user management with users/roles
            services.AddDbContext<ApplicationIdentityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IDPContextConnection")));

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
                config.Cookie.Name = ".AspNetCore.IDP_Cookie"; //This name must start with .AspNetCore. => This way the MCV project can find it and delete it when logout.
                //config.Cookie.Name = "IDP.Cookie";
                config.LoginPath = "/Auth/Login";
                config.LogoutPath = "/api/V01/Logout/Logout"; 
            });


            //Add the toolbox identityServer4. IE auth, open id connect, clients, apis, scopes
            //Register APIs and Clients that are alowed to access this IDP
            services.AddIdentityServer()
                .AddAspNetIdentity<ApplicationUser>()      //Glue is4 with core identity.
                .AddInMemoryApiResources(IdentityResourcesInMemoryRepo.GetApis())
                .AddInMemoryIdentityResources(IdentityResourcesInMemoryRepo.GetIdentityResources())
                .AddInMemoryClients(IdentityResourcesInMemoryRepo.GetClients())
                .AddDeveloperSigningCredential(); //Generate certificate to sign tokens. This replace the superSecretKey used in JWT-token projects.



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
            LogUtils.LogInMiddleware(app, "IDP", "UseStaticFiles", "UseHttpsRedirection");

            app.UseHttpsRedirection();
            LogUtils.LogInMiddleware(app, "IDP", "UseHttpsRedirection", "UseRouting");

            app.UseRouting();
            LogUtils.LogInMiddleware(app, "IDP", "UseRouting", "UseAuthentication");

            app.UseAuthentication(); //JN
            LogUtils.LogInMiddleware(app, "IDP", "UseAuthentication", "UseAuthorization");

            app.UseAuthorization(); //
            LogUtils.LogInMiddleware(app, "IDP", "UseAuthorization", "UseIdentityServer");

            app.UseIdentityServer(); //IdentityServer4 Nuget

            LogUtils.LogInMiddleware(app, "IDP", "UseIdentityServer", "UseEndpoints");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute(); // 
            });
            LogUtils.LogInMiddleware(app, "IDP", "EndStation", "EndStation");
        }
    }
}
