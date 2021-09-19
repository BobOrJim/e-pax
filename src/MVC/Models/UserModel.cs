using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string normalizedUserName { get; set; }
        //normalizedUserName: "ADMIN@ADMIN.COM"
        public string Password { get; set; }
    }
}
