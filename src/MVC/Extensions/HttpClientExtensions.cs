using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MVC.Extensions
{

    public static class HttpClientExtensions
    {
        //HttpClient httpClient = new();
        public static HttpClient HttpClientPrep(this HttpClient httpClient, string uri, string accessToken)
        {
            httpClient.BaseAddress = new Uri(uri);
            httpClient.SetBearerToken(accessToken);
            return httpClient;
        }

        //public static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
        //{
            //if (!response.IsSuccessStatusCode)
            //    throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");
            //var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            //return JsonSerializer.Deserialize<T>(dataAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        //}
    }


}

