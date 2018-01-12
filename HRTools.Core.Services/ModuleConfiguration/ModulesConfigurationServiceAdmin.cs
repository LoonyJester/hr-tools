using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using HRTools.Core.Common.Models;
using HRTools.Core.Repositories.Module_Configuration;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Master;

namespace HRTools.Core.Services.ModuleConfiguration
{
    public class ModulesConfigurationServiceAdmin : IModulesConfigurationServiceAdmin
    {
        private readonly IMapper _mapper;
        private readonly IModulesConfigurationRepositoryAdmin _modulesConfigurationRepositoryAdmin;
        private readonly ICacheConfigurationRepository _modulesCacheConfigurationRepository;

        public ModulesConfigurationServiceAdmin(
            IModulesConfigurationRepositoryAdmin modulesConfigurationRepositoryAdmin,
            ICacheConfigurationRepository modulesCacheConfigurationRepository,
            IMapper mapper)
        {
            Guard.ConstructorArgumentIsNotNull(modulesConfigurationRepositoryAdmin, nameof(modulesConfigurationRepositoryAdmin));
            Guard.ConstructorArgumentIsNotNull(modulesCacheConfigurationRepository, nameof(modulesCacheConfigurationRepository));
            Guard.ConstructorArgumentIsNotNull(mapper, nameof(mapper));

            _modulesConfigurationRepositoryAdmin = modulesConfigurationRepositoryAdmin;
            _modulesCacheConfigurationRepository = modulesCacheConfigurationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ModuleConfig>> GetAllAsync(Guid clientId, bool showOld)
        {
            Guard.ArgumentIsNotNull(clientId, nameof(clientId));

            IEnumerable<ModuleConfigDto> moduleConfigurationDtos =
                await _modulesConfigurationRepositoryAdmin.GetAllAsync(clientId, showOld);

            return _mapper.Map<IEnumerable<ModuleConfigDto>, IEnumerable<ModuleConfig>>(moduleConfigurationDtos);
        }

        public async Task<int> CreateAsync(ModuleConfig moduleConfig, List<ModuleConfig> actualModuleConfigs, Guid clientId)
        {
            Guard.ArgumentIsNotNull(moduleConfig, nameof(moduleConfig));

            if (!IsModuleConfigValid(moduleConfig, actualModuleConfigs))
            {
                return 0;
            }

            _modulesCacheConfigurationRepository.DeleteConfiguration(clientId);

            ModuleConfigDto dto = _mapper.Map<ModuleConfig, ModuleConfigDto>(moduleConfig);

            return await _modulesConfigurationRepositoryAdmin.CreateAsync(dto);
        }
        
        public Task<int> UpdateAsync(ModuleConfig moduleConfig, List<ModuleConfig> actualModuleConfigs, Guid clientId)
        {
            Guard.ArgumentIsNotNull(moduleConfig, nameof(moduleConfig));

            if (!IsModuleConfigValid(moduleConfig, actualModuleConfigs))
            {
                return Task.FromResult(0);
            }

            _modulesCacheConfigurationRepository.DeleteConfiguration(clientId);

            ModuleConfigDto dto = _mapper.Map<ModuleConfig, ModuleConfigDto>(moduleConfig);

            return _modulesConfigurationRepositoryAdmin.UpdateAsync(dto);
        }

        private static bool IsModuleConfigValid(ModuleConfig moduleConfig, List<ModuleConfig> actualModuleConfigs)
        {
            DateTime newStartDate = moduleConfig.StartDate.Date;
            DateTime? newEndDate = null;

            if (moduleConfig.EndDate != null)
            {
                newEndDate = moduleConfig.EndDate.Value.Date;
            }

            foreach (ModuleConfig actualModule in actualModuleConfigs)
            {
                if (actualModule.ModuleName != moduleConfig.ModuleName || (actualModule.Id == moduleConfig.Id))
                {
                    continue;
                }

                if (actualModule.StartDate < newStartDate)
                {
                    if (actualModule.EndDate == null || actualModule.EndDate > newStartDate)
                    {
                        return false;
                    }
                }
                else if (actualModule.StartDate > newStartDate)
                {
                    if (newEndDate == null || actualModule.StartDate < newEndDate)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public Task<int> StopAllAsync(List<ModuleConfig> actualModuleConfigs, Guid clientId)
        {
            Guard.ArgumentIsNotNull(clientId, nameof(clientId));
            
            _modulesCacheConfigurationRepository.DeleteConfiguration(clientId);

            int result = 0;

            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                foreach (ModuleConfig moduleConfig in actualModuleConfigs)
                {
                    ModuleConfigDto dto = _mapper.Map<ModuleConfig, ModuleConfigDto>(moduleConfig);
                    dto.EndDate = DateTime.UtcNow;

                    result += _modulesConfigurationRepositoryAdmin.UpdateAsync(dto).Result;
                }

                // todo: check
                if (result == actualModuleConfigs.Count)
                {
                    scope.Complete();
                }
            }

            return Task.FromResult(result);
        }
    }
}