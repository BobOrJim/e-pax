﻿using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIGateway1.Services
{
    public class TokenFactory : ITokenFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TokenFactory(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetAccessToken()
        {
            var IDPClient = _httpClientFactory.CreateClient();
            var discoveryDocument = await IDPClient.GetDiscoveryDocumentAsync("https://localhost:44327/");

            var TokenResponse = await IDPClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest //Flow = ClientCredentials, aka machine to machine
                {
                    Address = discoveryDocument.TokenEndpoint,
                    ClientId = "apigateway1",
                    ClientSecret = "apigateway1_secret",
                    Scope = "API_Forest",
                });

            return TokenResponse.AccessToken;
        }

    }
}