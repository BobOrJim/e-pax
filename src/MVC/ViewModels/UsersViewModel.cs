using MVC.Models;
using System.Collections.Generic;

namespace MVC.ViewModels
{
    public class UsersViewModel
    {
        public string Message { get; set; } = ""; //Any random DEV message we want to show in the View
        public string jsonSerializeStringPlaceholder1 { get; set; }
        public string jsonSerializeStringPlaceholder2 { get; set; }
        public string NewUserName { get; set; }
        public string NewPassword { get; set; }

        public string SearchPhrase { get; set; }
        public bool SortAlphabetically { get; set; }
        public List<UserModel> ListOfUsers { get; set; } = new List<UserModel>();
    }
}
