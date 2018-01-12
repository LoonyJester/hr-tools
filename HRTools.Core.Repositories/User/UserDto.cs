using System;
using System.Collections.Generic;
using HRTools.Crosscutting.Common.Models.User;

namespace HRTools.Core.Repositories.User
{
    public class UserDto
    {
        public Guid Subject { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }
        
        public List<Role> Roles { get; set; }

        public string FullName { get; set; }

        public bool IsActivated { get; set; }
    }
}
