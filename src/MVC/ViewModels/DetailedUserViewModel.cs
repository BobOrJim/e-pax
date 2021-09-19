using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.ViewModels
{
    public class DetailedUserViewModel
    {
        public string Message { get; set; } = ""; //Any random DEV message we want to show in the View
        public string UserName { get; set; } = "";
        //public List<string> AllRoles { get; set; } = new List<string>();
        //public List<string> UsersRoles { get; set; } = new List<string>();
        public List<UsersRolesModel> UsersRoles { get; set; } = new List<UsersRolesModel>();
    }
}


