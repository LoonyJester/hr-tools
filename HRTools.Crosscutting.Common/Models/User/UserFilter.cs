using System.Collections.Generic;

namespace HRTools.Crosscutting.Common.Models.User
{
    public class UserFilter
    {
        public List<Role> Roles { get; set; }

        public bool? IsActivated { get; set; }
    }
}
