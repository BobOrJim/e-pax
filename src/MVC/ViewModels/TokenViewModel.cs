using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.ViewModels
{
    public class TokenViewModel
    {

        public string AccessTokenHeader { get; set; }
        public string AccessTokenPayload { get; set; }
        public string AccessToken_nbf { get; set; }
        public string AccessToken_exp { get; set; }
        public string AccessToken_auth_time { get; set; }



        public string IdTokenHeader { get; set; }
        public string IdTokenPayload { get; set; }
        public string IdToken_nbf { get; set; }
        public string IdToken_exp { get; set; }
        public string IdToken_auth_time { get; set; }



        public string RefreshTokenHeader { get; set; }
        public string RefreshTokenPayload { get; set; }
        public string RefreshToken_nbf { get; set; }
        public string RefreshToken_exp { get; set; }
        public string RefreshToken_auth_time { get; set; }



        public string TimeStampLatestUpdate { get; set; }

    }
}
