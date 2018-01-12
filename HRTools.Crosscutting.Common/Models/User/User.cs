using System.Collections.Generic;

namespace HRTools.Crosscutting.Common.Models.User
{
    public class User
    {
        public string UserId { get; set; }

        // Login is Email
        public string Login { get; set; }

        public string Password { get; set; }
        
        public List<Role> Roles { get; set; }

        public string FullName { get; set; }

        public bool IsActivated { get; set; }

        public string AssignedEmployeeName { get; set; }
    }
}