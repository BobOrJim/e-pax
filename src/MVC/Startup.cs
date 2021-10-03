using Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MVC
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //Vi kommer få både access och id token.
            services.AddAuthentication(config => {
                config.DefaultScheme = "mvc_client_cookie"; //Vi kör aspnet default förutom utmaningen nedan
                config.DefaultChallengeScheme = "oidc"; //Här kör vi oicd
            })
                .AddCookie("mvc_client_cookieC1") //JN trying to remove this cookie when logout
                .AddCookie("mvc_client_cookieC2") //JN trying to remove this cookie when logout
                .AddCookie("IDP_Cookie") //JN trying to remove this cookie when logout
                .AddCookie("mvc_client_cookie")
                .AddOpenIdConnect("oidc", config => { //denna accessar discovery document. 
                    config.Authority = uri.IDP;
                    config.ClientId = "client_mvc";
                    config.ClientSecret = "client_secret_mvc";
                    config.SaveTokens = true;
                    config.ResponseType = "code";  //Detta (code = authorization code flow) gör att vi får en code tillbacks av IDP, denna code används sedan för att byta till oss en id och access token. Troligen sker byte hos mvc backend.
                                                   //Om vi använder implicit flow får vi id/access tokens direkt, dessa har lägre säkerhet och är på väg bort från marknaden.
                    config.SignedOutCallbackPath = "/Home/Index";

                    // configure cookie claim mapping
                    config.ClaimActions.DeleteClaim("amr");
                    config.ClaimActions.DeleteClaim("s_hash");
                    //config.ClaimActions.MapUniqueJsonKey("RawCoding.Grandma", "rc.garndma");
                    config.ClaimActions.MapJsonKey("role", "role");

                    // two trips to load claims in to the cookie
                    // but the id token is smaller !
                    config.GetClaimsFromUserInfoEndpoint = true; //Efter vi har fått ID token, kommer Access token efterfrågas där våra claims ligger

                    //Configre scope
                    config.Scope.Clear();
                    config.Scope.Add("openid");
                    config.Scope.Add("profile");
                    config.Scope.Add("APIGateway1");
                    config.Scope.Add("APIGateway2");
                    config.Scope.Add("offline_access"); //Token refresh
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


            app.UseStaticFiles(); //Enable bootstrat etc

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
