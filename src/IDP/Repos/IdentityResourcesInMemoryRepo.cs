using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityModel;
using IdentityServer4;

namespace IDP.Repos
{
    public static class IdentityResourcesInMemoryRepo
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            //Map things to IdentityToken
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiResource> GetApis() =>
            //Map things to accesstoken
            new List<ApiResource>{
                new ApiResource("APIGateway1", new string[]{ "role" }),
                new ApiResource("APIGateway2", new string[]{ "role" }),
                new ApiResource("API_Forest", new string[]{ "role" }),
                new ApiResource("API_Mountain", new string[]{ "role" }),
                new ApiResource("API_Desert", new string[]{ "role" }),
                new ApiResource("IDP", new string[]{ "role" }), //JN

            };

        //For is4.1.x, laborera senare. Nu kör jag is3.xxx
        //public static IEnumerable<ApiScope> GetApiScopes() =>
        //    new List<ApiScope> {
        //        new ApiScope("ApiOne")
        //    };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                 new Client {
                    ClientId = "client_APIGateway1",
                    ClientSecrets = { new Secret("APIGateway1_secret".ToSha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,                  //Flow. Machine to Machine
                    AllowedScopes = { "API_Forest", "API_Mountain"},           //client_APIGateway1 are allowed to acces these scopes.
                },

                 new Client {
                    ClientId = "client_APIGateway2",
                    ClientSecrets = { new Secret("APIGateway2_secret".ToSha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,           
                    AllowedScopes = { "API_Desert" },                           
                },

                new Client { //This client has a user, ie we would like and identity + access token. 
                    ClientId = "client_mvc",
                    ClientSecrets = { new Secret("client_secret_mvc".ToSha256()) },
                    AllowedGrantTypes = GrantTypes.Code,           //Flow. "human" to machine
                    RedirectUris = { "https://localhost:44345/signin-oidc" }, //MVC. We send the client back, so they can exchange the code for a token.
                    PostLogoutRedirectUris = { "https://localhost:44345/" }, //MVC. Redirect user back home
                    AllowedScopes = {
                        "APIGateway1",
                        "APIGateway2",
                        IdentityServerConstants.StandardScopes.OpenId, //Add open ID layer, and give user and id token.
                        IdentityServerConstants.StandardScopes.Profile,
                    },

                    // puts all the claims in the id token
                    AlwaysIncludeUserClaimsInIdToken = true,

                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 1, //The final time is 5min + this setting.
                    RequireConsent = false,
                    IdentityTokenLifetime = 1, //The final time is 5min + this setting.
                    AbsoluteRefreshTokenLifetime = 3600,
                    AuthorizationCodeLifetime = 300, //Time to exchange code for tokens befor code become invalid.
                }
            };
    }
}
