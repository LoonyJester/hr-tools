using System.Data;
using System.Configuration;
using System.Data.Common;
using HRTools.Crosscutting.Common.Master;
using MySql.Data.MySqlClient;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace HRTools.Crosscutting.Common.DataAccess
{
    public class MasterConnectionFactory : IConnectionFactory
    {
        private readonly DbProviderFactory _provider;
        private readonly string _connectionString;
        private readonly string _name;

        private const string ConnectionName = "MySqlConnectionString";

        public MasterConnectionFactory()
        {
            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[ConnectionName];

            if (connectionStringSettings == null)
            {
                throw new ConfigurationErrorsException(
                    $"Failed to find connection string named '{ConnectionName}' in app/web.config.");
            }

            _name = connectionStringSettings.ProviderName;
            _provider = DbProviderFactories.GetFactory(connectionStringSettings.ProviderName);
            _connectionString = connectionStringSettings.ConnectionString;
        }

        public IDbConnection Create(ClientConfiguration configuration)
        {
            IDbConnection connection = MySqlClientFactory.Instance.CreateConnection();// _provider.CreateConnection();
            if (connection == null)
            {
                throw new ConfigurationErrorsException(
                    $"Failed to create a connection using the connection string named '{_name}' in app/web.config.");
            }

            connection.ConnectionString = _connectionString;

            return connection;
        }
    }
}
