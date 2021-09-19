using MVC.Models;
using System.Collections.Generic;

namespace MVC.ViewModels
{
    public class RolesViewModel
    {
        public string Message { get; set; } = ""; //Any random DEV message we want to show in the View
        public string jsonSerializeStringPlaceholder1 { get; set; }
        public string jsonSerializeStringPlaceholder2 { get; set; }
        public string NewRoleName { get; set; }
        public string SearchPhrase { get; set; }
        public bool SortAlphabetically { get; set; }
        public List<RoleModel> ListOfRoles { get; set; } = new List<RoleModel>();
    }
}
