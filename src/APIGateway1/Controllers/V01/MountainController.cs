using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using APIGateway1.Services;
using Microsoft.AspNetCore.Authorization;
using Common;
using Common.Extensions;
using System.Net;

namespace APIGateway1.Controllers.V01
{
    [ApiController]
    [Route("api/V01/[controller]")]
    public class MountainController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenFactory _tokenFactory;

        public MountainController(IHttpClientFactory httpClientFactory, ITokenFactory tokenFactory)
        {
            _httpClientFactory = httpClientFactory;
            _tokenFactory = tokenFactory;
        }

        [HttpGet("GetSecretMountainInEurope")]
        [Authorize(Roles = "Admin, Masters_Degree_In_Mining")]
        public async Task<IActionResult> GetSecretMountainInEurope()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var API_MountainClient = _httpClientFactory.CreateClient().HttpClientPrep(uri.API_Mountain, await _tokenFactory.GetAccessToken());
            var SecretResponse = await API_MountainClient.GetAsync("api/V01/Mountains/SecretMountainInEurope");
            return Ok(await SecretResponse.Content.ReadAsStringAsync());

        }
    }
}


