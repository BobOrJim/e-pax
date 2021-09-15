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
        public double AccessTokenLifeLeftPercent { get; set; }


        public string IdTokenHeader { get; set; }
        public string IdTokenPayload { get; set; }
        public string IdToken_nbf { get; set; }
        public string IdToken_exp { get; set; }
        public string IdToken_auth_time { get; set; }
        public double IdTokenLifeLeftPercent { get; set; }


        public string RefreshTokenCode { get; set; }


        public string TimeStampLatestUpdate { get; set; }

    }
}
