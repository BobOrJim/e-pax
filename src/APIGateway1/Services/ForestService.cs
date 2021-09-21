using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIGateway1.Services
{
    public class ForestService : IForestService
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenFactory _tokenFactory;

        public ForestService(IHttpClientFactory httpClientFactory, ITokenFactory tokenFactory)
        {
            _httpClientFactory = httpClientFactory;
            _tokenFactory = tokenFactory;
        }

        public async Task<string> CallEndpoint(string url)
        {
            var CallAPI1Client = _httpClientFactory.CreateClient();
            var token = await _tokenFactory.GetAccessToken();
            CallAPI1Client.SetBearerToken(token);
            var SecretResponse = await CallAPI1Client.GetAsync(url);
            var responseMessage = await SecretResponse.Content.ReadAsStringAsync();
            return responseMessage;
        }

        //private async Task<string> AccessTokenFactory()
        //{
        //    var IDPClient = _httpClientFactory.CreateClient();
        //    var discoveryDocument = await IDPClient.GetDiscoveryDocumentAsync("https://localhost:44327/");

        //    var TokenResponse = await IDPClient.RequestClientCredentialsTokenAsync(
        //        new ClientCredentialsTokenRequest //Flow = ClientCredentials, aka machine to machine
        //        {
        //            Address = discoveryDocument.TokenEndpoint,
        //            ClientId = "apigateway1",
        //            ClientSecret = "apigateway1_secret",
        //            Scope = "API_Forest",
        //        });

        //    return TokenResponse.AccessToken;
        //}


    }
}


