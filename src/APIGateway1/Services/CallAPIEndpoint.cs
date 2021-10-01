using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIGateway1.Services
{
    public class CallAPIEndpoint : ICallAPIEndpoint
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenFactory _tokenFactory;

        public CallAPIEndpoint(IHttpClientFactory httpClientFactory, ITokenFactory tokenFactory)
        {
            _httpClientFactory = httpClientFactory;
            _tokenFactory = tokenFactory;
        }

        public async Task<string> CallEndpoint(string url)
        {
            var CallClient = _httpClientFactory.CreateClient();
            var token = await _tokenFactory.GetAccessToken();
            CallClient.SetBearerToken(token);
            var SecretResponse = await CallClient.GetAsync(url);
            var responseMessage = await SecretResponse.Content.ReadAsStringAsync();
            return responseMessage;
        }
    }
}


