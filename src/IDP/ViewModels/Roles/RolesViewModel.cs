using IDP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDP.ViewModels.Roles
{
    public class RolesViewModel
    {
        public string Message { get; set; } = "";
        public string jsonSerializeStringPlaceholder1 { get; set; }
        public string jsonSerializeStringPlaceholder2 { get; set; }
        public string NewRoleName { get; set; }
        public bool AddRole { get; set; }
        public string SearchPhrase { get; set; }

        public List<RoleModel> ListOfRoles { get; set; } = new List<RoleModel>();
        public List<RoleModel> SortedListOfRoles { get; set; }

    }
}
