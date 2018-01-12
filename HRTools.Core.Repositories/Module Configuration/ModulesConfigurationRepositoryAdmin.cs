using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.DataAccess;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Crosscutting.Common.Master;

namespace HRTools.Core.Repositories.Module_Configuration
{
    public class ModulesConfigurationRepositoryAdmin : Repository, IModulesConfigurationRepositoryAdmin
    {
        public ModulesConfigurationRepositoryAdmin(IConnectionFactory connectionFactory,
            IConfigurationManager configurationManager, ILogger logger)
            : base(connectionFactory, configurationManager, logger)
        {
        }
        
        public Task<IEnumerable<ModuleConfigDto>> GetAllAsync(Guid clientId, bool showOld)
        {
            Guard.ArgumentIsNotNull(clientId, nameof(clientId));

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("ClientId", clientId);
            parameters.Add("ShowOld", showOld);

            return QueryAsync<ModuleConfigDto>(StoredProcedures.ModulesConfiguration.GetAll, parameters);
        }

        public Task<int> CreateAsync(ModuleConfigDto moduleConfig)
        {
            Guard.ArgumentIsNotNull(moduleConfig, nameof(moduleConfig));

            DynamicParameters parameters = new DynamicParameters();
            
            parameters.Add("ClientId", moduleConfig.ClientId);
            parameters.Add("ModuleName", moduleConfig.ModuleName);
            parameters.Add("StartDate", moduleConfig.StartDate);
            parameters.Add("EndDate", moduleConfig.EndDate);

            return QueryFirstOrDefaultAsync<int>(StoredProcedures.ModulesConfiguration.Create, parameters);
        }

        public Task<int> UpdateAsync(ModuleConfigDto moduleConfig)
        {
            Guard.ArgumentIsNotNull(moduleConfig, nameof(moduleConfig));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("Id", moduleConfig.Id);
            parameters.Add("ClientId", moduleConfig.ClientId);
            parameters.Add("ModuleName", moduleConfig.ModuleName);
            parameters.Add("StartDate", moduleConfig.StartDate);
            parameters.Add("EndDate", moduleConfig.EndDate);

            return ExecuteAsync(StoredProcedures.ModulesConfiguration.Update, parameters);
        }
    }
}