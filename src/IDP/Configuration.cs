using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityModel;


namespace IDP
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            //Här mappar vi till IdentityToken, med ApiResorce nedan mappar vi till accessToken.
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                //new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "rc.scope",
                    UserClaims =
                    {
                        "rc.garndma"
                    }
                },
                //new IdentityResource("roles", new[] { "role" }),
                //new IdentityResource //Vi deklarerar att detta är ett möjligt scope som andra program kan requesta
                //{
                //    //Name = "IDP.Configuration.GetIdentityResources.Scope",
                //    //UserClaims = 
                //    //{
                //    //    "IDP.Configuration.GetIdentityResources.UserClaims"
                //    //}
                //}
            };

        public static IEnumerable<ApiResource> GetApis() =>
            //Här anges tillåtna claims i AccessToken.
            new List<ApiResource>{
                new ApiResource("api1", new string[]{ "rc.api.garndma", "claimname", "role" } ), //Note, denna claim kan användas av både api1 o api2, dvs den är inte unik för api1
                new ApiResource("api2"),
            };

        //Krävs för is4.1.x, laborera senare. Nu kör jag is3.xxx
        //public static IEnumerable<ApiScope> GetApiScopes() =>
        //    new List<ApiScope> {
        //        new ApiScope("ApiOne")
        //    };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client { 
                    ClientId = "client_api2",
                    ClientSecrets = { new Secret("client_secret".ToSha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,           //Flow. Dvs för machine to Machine
                    AllowedScopes = { "api1" }                                  //program som får access till API1, notera att detta kompleteras med finmaskinare nät baserat på users via Core Identity. Dvs två parallella system.
                },
                new Client { //Denna klient har en user, dvs vi önskar identity token + access token
                    ClientId = "client_mvc",
                    ClientSecrets = { new Secret("client_secret_mvc".ToSha256()) },
                    AllowedGrantTypes = GrantTypes.Code,           //Flow. dvs "human" to machine
                    RedirectUris = { "https://localhost:44345/signin-oidc" },
                    AllowedScopes = { 
                        "api1", 
                        "api2", 
                        IdentityServer4.IdentityServerConstants.StandardScopes.OpenId, //Gör så vi får identity token också. Lägger på open id lager.
                        //IdentityServer4.IdentityServerConstants.StandardScopes.Profile, //Gör så vi får identity token också. Lägger på open id lager.
                        "rc.scope",

                    },             //program som får access till API1, notera att detta kompleteras med finmaskinare nät baserat på users via Core Identity. Dvs två parallella system.

                    //put all the claims in the id token
                    //AlwaysIncludeUserClaimsInIdToken = true,

                    RequireConsent = false,
                }
            };


    }
}
