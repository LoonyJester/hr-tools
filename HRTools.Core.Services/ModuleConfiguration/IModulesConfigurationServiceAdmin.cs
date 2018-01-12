using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRTools.Core.Common.Models;

namespace HRTools.Core.Services.ModuleConfiguration
{
    public interface IModulesConfigurationServiceAdmin
    {
        Task<IEnumerable<ModuleConfig>> GetAllAsync(Guid clientId, bool showOld);

        Task<int> CreateAsync(ModuleConfig moduleConfig, List<ModuleConfig> actualModuleConfigs, Guid clientId);

        Task<int> UpdateAsync(ModuleConfig moduleConfig, List<ModuleConfig> actualModuleConfigs, Guid clientId);

        Task<int> StopAllAsync(List<ModuleConfig> actualModuleConfigs, Guid clientId);
    }
}