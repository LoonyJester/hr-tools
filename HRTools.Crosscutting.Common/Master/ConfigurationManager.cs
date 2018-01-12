using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using HRTools.Crosscutting.Common.Helpers;

namespace HRTools.Crosscutting.Common.Master
{
    public class ConfigurationManager: IConfigurationManager
    {
        private readonly ICacheConfigurationRepository _cacheConfigurationRepository;
        private readonly IDbConfigurationRepository _dbConfigurationRepository;

        public ConfigurationManager(ICacheConfigurationRepository cacheConfigurationRepository,
            IDbConfigurationRepository dbConfigurationRepository)
        {
            Guard.ConstructorArgumentIsNotNull(cacheConfigurationRepository, nameof(cacheConfigurationRepository));
            Guard.ConstructorArgumentIsNotNull(dbConfigurationRepository, nameof(dbConfigurationRepository));

            _cacheConfigurationRepository = cacheConfigurationRepository;
            _dbConfigurationRepository = dbConfigurationRepository;
        }

        public async Task CreateConfigurationInCacheAsync()
        {
            List<ClientConfigurationDto> configurationList = await _dbConfigurationRepository.GetAllAsync();

            foreach (ClientConfigurationDto clientConfigurationDto in configurationList)
            {
                List<string> activeModules = await _dbConfigurationRepository.GetActiveModulesByClientIdAsync(clientConfigurationDto.ClientId);

                clientConfigurationDto.ActiveModules = string.Join(",", activeModules);
            }
            
            _cacheConfigurationRepository.CreateConfigurationList(configurationList);
        }

        public async Task<ClientConfiguration> GetConfigurationAsync()
        {
            // IIS web site names should be predefined correctly
            string host = HostingEnvironment.ApplicationHost.GetSiteName();
            Guid clientId = CompanyHelper.GetCompanyIdByHost(host);

            ClientConfigurationDto configurationDto = _cacheConfigurationRepository.GetByClientId(clientId);

            if (configurationDto != null)
            {
                return MapClientConfigurationDtoToClientConfiguration(configurationDto);
            }

            configurationDto = await _dbConfigurationRepository.GetByClientIdAsync(clientId);

            List<string> activeModules = await _dbConfigurationRepository.GetActiveModulesByClientIdAsync(clientId);

            if (configurationDto == null || activeModules == null || activeModules.Count == 0)
            {
                throw new ConfigurationErrorsException($"Configuration for the {host} was not found");
            }

            configurationDto.ActiveModules = string.Join(",", activeModules);

            _cacheConfigurationRepository.CreateConfiguration(configurationDto);
            
            return MapClientConfigurationDtoToClientConfiguration(configurationDto);
        }

        private static ClientConfiguration MapClientConfigurationDtoToClientConfiguration(ClientConfigurationDto configurationDto)
        {
            return new ClientConfiguration
            {
                ClientId = configurationDto.ClientId,
                ConnectionString = configurationDto.ConnectionString,
                ActiveModules = configurationDto.ActiveModules?.Split(',').ToList() ?? new List<string>()
            };
        }
    }
}