using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //Vi kommer f� b�de access och id token.
            services.AddAuthentication(config => {
                config.DefaultScheme = "mvc_client_cookie"; //Vi k�r aspnet default f�rutom utmaningen nedan
                config.DefaultChallengeScheme = "oidc"; //H�r k�r vi oicd

            })
                .AddCookie("mvc_client_cookie")
                .AddOpenIdConnect("oidc", config => { //denna accessar discovery document. 
                    config.Authority = "https://localhost:44327/";
                    config.ClientId = "client_mvc";
                    config.ClientSecret = "client_secret_mvc";
                    config.SaveTokens = true;
                    config.ResponseType = "code";  //Detta (code = authorization code flow) g�r att vi f�r en code tillbacks av IDP, denna code anv�nds sedan f�r att byta till oss en id och access token. Troligen sker byte hos mvc backend.
                                                   //Om vi anv�nder implicit flow f�r vi id/access tokens direkt, dessa har l�gre s�kerhet och �r p� v�g bort fr�n marknaden.

                    // configure cookie claim mapping
                    config.ClaimActions.DeleteClaim("amr");
                    config.ClaimActions.DeleteClaim("s_hash");
                    config.ClaimActions.MapUniqueJsonKey("RawCoding.Grandma", "rc.garndma");

                    // two trips to load claims in to the cookie
                    // but the id token is smaller !
                    config.GetClaimsFromUserInfoEndpoint = true; //Efter vi har f�tt ID token, kommer Access token efterfr�gas d�r v�ra claims ligger

                    //Configre scope
                    config.Scope.Clear();
                    config.Scope.Add("openid");
                    config.Scope.Add("rc.scope");
                    config.Scope.Add("api1");
                    config.Scope.Add("api2");


                });

            services.AddHttpClient();

            services.AddControllersWithViews();
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
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
