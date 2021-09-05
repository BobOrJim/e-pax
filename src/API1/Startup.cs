using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API1
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", config =>
                {
                    config.Authority = "https://localhost:44327/"; //Hitt kan API skicka access tokens f�r att validera dem.
                    config.Audience = "api1"; //apiOne identifierar sig sj�lv n�r denna �nskar validera token.
                });

            services.AddAuthorization(options =>
            {
                //options.AddPolicy("EmployeeOnly", policy => policy.RequireClaim("big.api.cookie"));
                //options.AddPolicy("EmployeeOnly", policy => policy.RequireClaim("apelsin"));
                options.AddPolicy("EmployeeOnly", builder => builder.RequireClaim("claimname", "claimvalue"));
            });

            services.AddControllers(); //Inga views h�r, bara controller.
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
