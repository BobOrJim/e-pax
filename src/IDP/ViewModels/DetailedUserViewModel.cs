using IDP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDP.ViewModels
{
    public class DetailedUserViewModel
    {
        public string Message { get; set; } = ""; 
        public string UserName { get; set; } = "";

        public List<UsersRolesModel> UsersRoles { get; set; } = new List<UsersRolesModel>();
    }
}


