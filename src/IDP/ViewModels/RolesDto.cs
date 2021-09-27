using IDP.Models;
using System.Collections.Generic;

namespace IDP.ViewModels
{
    public class RolesDto
    {
        public string Message { get; set; } = ""; 
        public string jsonSerializeStringPlaceholder1 { get; set; }
        public string jsonSerializeStringPlaceholder2 { get; set; }
        public string NewRoleName { get; set; }
        public string SearchPhrase { get; set; }
        public bool SortAlphabetically { get; set; }
        public List<RoleModel> ListOfRoles { get; set; } = new List<RoleModel>();
    }
}
