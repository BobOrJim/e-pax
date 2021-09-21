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

    }
}


