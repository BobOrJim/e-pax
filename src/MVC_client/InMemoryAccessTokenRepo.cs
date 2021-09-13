using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_client
{
    public static class InMemoryAccessTokenRepo
    {
        public static string AccessToken { get; private set; }
        public static Int64 TimestampLastAccessTokenUpdate { get; set; }

        public static void SetAccessToken(string TokenString)
        {
            if (TokenString != AccessToken)
            {
                AccessToken = TokenString;
                TimestampLastAccessTokenUpdate = DateTime.Now.Ticks;
            }
        }
    }
}
