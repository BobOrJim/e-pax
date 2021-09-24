using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway1.Constants
{
    public static class API_Endpoint
    {
        public static string SecretForestInEurope { get; } = "https://localhost:44380/api/V01/Forests/SecretForestInEurope";

        public static string SecretMountainInEurope { get; } = "https://localhost:44374/api/V01/Mountains/SecretMountainInEurope";
                                                                
    }
}

