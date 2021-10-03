using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using APIGateway1.Services;
using Microsoft.AspNetCore.Authorization;
using Common;
using Common.Extensions;

namespace APIGateway1.Controllers.V01
{
    [ApiController]
    [Route("api/V01/[controller]")]
    public class ForestController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenFactory _tokenFactory;

        public ForestController(IHttpClientFactory httpClientFactory, ITokenFactory tokenFactory)
        {
            _httpClientFactory = httpClientFactory;
            _tokenFactory = tokenFactory;
        }

        [HttpGet("GetSecretForestInEurope")]
        [Authorize(Roles = "Admin, Masters_Degree_In_Forestry")]
        public async Task<IActionResult> GetSecretForestInEurope()
        {
            var API_ForestClient = _httpClientFactory.CreateClient().HttpClientPrep(uri.API_Forest, _tokenFactory.GetAccessToken().GetAwaiter().GetResult());
            var SecretResponse = await API_ForestClient.GetAsync("api/V01/Forests/SecretForestInEurope");
            return Ok(await SecretResponse.Content.ReadAsStringAsync());
        }
    }
}


