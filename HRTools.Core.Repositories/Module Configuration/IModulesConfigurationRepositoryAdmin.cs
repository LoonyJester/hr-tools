using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRTools.Core.Repositories.Module_Configuration
{
    public interface IModulesConfigurationRepositoryAdmin
    {
        Task<IEnumerable<ModuleConfigDto>> GetAllAsync(Guid clientId, bool showOld);

        Task<int> CreateAsync(ModuleConfigDto moduleConfig);

        Task<int> UpdateAsync(ModuleConfigDto moduleConfig);
    }
}
