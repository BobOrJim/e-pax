using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Repos
{
    public static class InMemoryTokenRepo
    {
        public static string AccessToken { get; private set; }
        public static string IdToken { get; private set; }
        public static string RefreshToken { get; private set; }
        public static double AccessTokenLifeLeftPercent { get; set; }
        public static double IdTokenLifeLeftPercent { get; set; }
        public static Int64 TimestampLastAccessTokenUpdate { get; set; }

        public static void SetAccessToken(string TokenString)
        {
            if (TokenString != AccessToken)
            {
                AccessToken = TokenString;
                TimestampLastAccessTokenUpdate = DateTime.Now.Ticks;
            }
        }

        public static void SetIdToken(string TokenString)
        {
            if (TokenString != IdToken)
            {
                IdToken = TokenString;
                TimestampLastAccessTokenUpdate = DateTime.Now.Ticks;
            }
        }

        public static void SetRefreshToken(string TokenString)
        {
            if (TokenString != RefreshToken)
            {
                RefreshToken = TokenString;
                TimestampLastAccessTokenUpdate = DateTime.Now.Ticks;
            }
        }


    }
}

