using System.Data.Entity;
using HRTools.AuthorizationServer.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HRTools.AuthorizationServer
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext(string connectionString)
            : base(connectionString)
        {

        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}