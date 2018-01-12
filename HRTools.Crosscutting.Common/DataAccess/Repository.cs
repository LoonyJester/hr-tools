using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Crosscutting.Common.Master;

namespace HRTools.Crosscutting.Common.DataAccess
{
    public abstract class Repository
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger _logger;

        protected Repository(IConnectionFactory connectionFactory,
            IConfigurationManager configurationManager,
            ILogger logger)
        {
            Guard.ConstructorArgumentIsNotNull(connectionFactory, nameof(connectionFactory));
            Guard.ConstructorArgumentIsNotNull(configurationManager, nameof(configurationManager));
            Guard.ConstructorArgumentIsNotNull(logger, nameof(logger));

            _connectionFactory = connectionFactory;
            _configurationManager = configurationManager;
            _logger = logger;
        }

        protected async Task<IEnumerable<TResult>> QueryAsync<TResult>(string storedProcedureName,
            DynamicParameters parameters)
        {
            _logger.Debug($"SP name: {storedProcedureName}, params: {string.Join(",", parameters.ParameterNames)}.");

            Guard.ArgumentIsNotNullOrEmpty(storedProcedureName, nameof(storedProcedureName));
            Guard.ArgumentIsNotNull(parameters, nameof(parameters));

            return await TryOpenConnectionAsync(connection =>
            {
                Task<IEnumerable<TResult>> queryResult = connection.QueryAsync<TResult>(storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return queryResult;
            });
        }

        protected async Task<TResult> QuerySingleOrDefaultAsync<TResult>(string storedProcedureName,
            DynamicParameters parameters)
        {
            _logger.Debug($"SP name: {storedProcedureName}, params: {string.Join(",", parameters.ParameterNames)}.");

            Guard.ArgumentIsNotNullOrEmpty(storedProcedureName, nameof(storedProcedureName));
            Guard.ArgumentIsNotNull(parameters, nameof(parameters));

            return await TryOpenConnectionAsync(connection =>
            {
                Task<TResult> queryResult = connection.QuerySingleOrDefaultAsync<TResult>(storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return queryResult;
            });
        }

        protected async Task<TResult> QueryFirstOrDefaultAsync<TResult>(string storedProcedureName,
            DynamicParameters parameters)
        {
            _logger.Debug($"SP name: {storedProcedureName}, params: {string.Join(",", parameters.ParameterNames)}.");

            Guard.ArgumentIsNotNullOrEmpty(storedProcedureName, nameof(storedProcedureName));
            Guard.ArgumentIsNotNull(parameters, nameof(parameters));

            return await TryOpenConnectionAsync(connection =>
            {
                Task<TResult> queryResult = connection.QueryFirstOrDefaultAsync<TResult>(storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return queryResult;
            });
        }

        protected async Task<int> ExecuteAsync(string storedProcedureName, DynamicParameters parameters)
        {
            _logger.Debug($"SP name: {storedProcedureName}, params: {string.Join(",", parameters.ParameterNames)}.");

            Guard.ArgumentIsNotNullOrEmpty(storedProcedureName, nameof(storedProcedureName));
            Guard.ArgumentIsNotNull(parameters, nameof(parameters));

            return await TryOpenConnectionAsync(connection =>
            {
                Task<int> executeResult = connection.ExecuteAsync(storedProcedureName,
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return executeResult;
            });
        }

        private async Task<TResult> TryOpenConnectionAsync<TResult>(Func<IDbConnection, Task<TResult>> func)
        {
            try
            {
                ClientConfiguration configuration = await _configurationManager.GetConfigurationAsync();

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