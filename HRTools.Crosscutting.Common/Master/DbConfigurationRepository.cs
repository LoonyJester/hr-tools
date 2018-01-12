using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using HRTools.Crosscutting.Common.DataAccess;
using HRTools.Crosscutting.Common.Logging;

namespace HRTools.Crosscutting.Common.Master
{
    public class DbConfigurationRepository : IDbConfigurationRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger _logger;

        public DbConfigurationRepository(IConnectionFactory connectionFactory, ILogger logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public Task<List<ClientConfigurationDto>> GetAllAsync()
        {
            DynamicParameters parameters = new DynamicParameters();

            return QueryAsync<ClientConfigurationDto>("master_configuration_getAll", parameters);
        }

        public Task<ClientConfigurationDto> GetByClientIdAsync(Guid clientId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("ClientId", clientId);

            return QuerySingleOrDefaultAsync<ClientConfigurationDto>("master_configuration_getByClientId", parameters);
        }

        public Task<List<string>> GetActiveModulesByClientIdAsync(Guid clientId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("ClientId", clientId);

            return QueryAsync<string>("master_modulesconfiguration_getActiveModulesByClientId", parameters);
        }

        private async Task<List<TResult>> QueryAsync<TResult>(string storedProcedureName,
            DynamicParameters parameters)
        {
            _logger.Debug($"SP name: {storedProcedureName}, params: {string.Join(",", parameters.ParameterNames)}.");

            return await TryOpenConnectionAsync(async connection =>
            {
                Guard.ArgumentIsNotNullOrEmpty(storedProcedureName, nameof(storedProcedureName));
                Guard.ArgumentIsNotNull(parameters, nameof(parameters));

                List<TResult> queryResult = (await connection.QueryAsync<TResult>(storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                )).ToList();

                return queryResult;
            });
        }

        private async Task<TResult> QuerySingleOrDefaultAsync<TResult>(string storedProcedureName,
            DynamicParameters parameters)
        {
            _logger.Debug($"SP name: {storedProcedureName}, params: {string.Join(",", parameters.ParameterNames)}.");

            return await TryOpenConnectionAsync(async connection =>
            {
                Guard.ArgumentIsNotNullOrEmpty(storedProcedureName, nameof(storedProcedureName));
                Guard.ArgumentIsNotNull(parameters, nameof(parameters));

                Task<TResult> queryResult = connection.QuerySingleOrDefaultAsync<TResult>(storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return await queryResult;
            });
        }

        private async Task<TResult> TryOpenConnectionAsync<TResult>(Func<IDbConnection, Task<TResult>> func)
        {
            try
            {
                ClientConfiguration configuration = new ClientConfiguration();

                using (IDbConnection connection = _connectionFactory.Create(configuration))
                {
                    _logger.Debug($"Connection string: {connection.ConnectionString}.");

                    connection.Open();

                    _logger.Debug("Connection is opened.");

                    return await func(connection);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                throw;
            }
            finally
            {
                _logger.Debug("Connection is closed.");
            }
        }
    }
}