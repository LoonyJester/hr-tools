using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using HRTools.Crosscutting.Common.Master;

namespace HRTools.Crosscutting.Common.DataAccess
{
    public class DefaultConnectionFactory : IConnectionFactory
    {
        public IDbConnection Create(ClientConfiguration configuration)
        {
            Guard.ArgumentIsNotNull(configuration, nameof(configuration));
            Guard.ArgumentIsNotNullOrEmpty(configuration.ClientId, nameof(configuration.ClientId));
            
            if (string.IsNullOrWhiteSpace(configuration.ConnectionString))
            {
                throw new ConfigurationErrorsException(
                    $"Failed to find connection string for a client '{configuration.ClientId}' in app/web.config.");
            }

            IDbConnection connection = MySqlClientFactory.Instance.CreateConnection();

            if (connection == null)
            {
                throw new Exception($"Failed to create a connection using the connection for a client id '{configuration.ClientId}' in app/web.config.");
            }

            connection.ConnectionString = configuration.ConnectionString;

            return connection;
        }
    }
}
