using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIGateway1.Services
{
    public class ForestService
    {
        private readonly HttpClient client;

        public ForestService(HttpClient client)
        {
            this.client = client;
        }

    }
}


