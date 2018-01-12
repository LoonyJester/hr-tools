using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HRTools.Crosscutting.Common.Master
{
    public class CacheConfigurationRepository: ICacheConfigurationRepository
    {
        private const int FiveMunutesInMiliseconds = 60 * 5 * 1000;

        public ClientConfigurationDto GetByClientId(Guid clientId)
        {
            MemoryCache memoryCache = MemoryCache.Default;

            object valueFromCache = memoryCache.Get(clientId.ToString());

            if (valueFromCache == null)
            {
                return null;
            }

            string value = valueFromCache.ToString();

            ClientConfigurationDto configurationDto = JsonConvert.DeserializeObject<ClientConfigurationDto>(value);

            return configurationDto;
        }

        public void CreateConfigurationList(List<ClientConfigurationDto> configurations)
        {
            foreach (ClientConfigurationDto configuration in configurations)
            {
                CreateConfiguration(configuration);
            }
        }

        public Task<bool> CreateConfiguration(ClientConfigurationDto configuration)
        {
            MemoryCache memoryCache = MemoryCache.Default;

            string config = JsonConvert.SerializeObject(configuration);

            return Task.FromResult(memoryCache.Add(configuration.ClientId.ToString(), config, DateTimeOffset.UtcNow.AddMilliseconds(FiveMunutesInMiliseconds)));
        }

        public bool DeleteConfiguration(Guid clientId)
        {
            Guard.ArgumentIsNotNull(clientId, nameof(clientId));

            MemoryCache memoryCache = MemoryCache.Default;

            return memoryCache.Remove(clientId.ToString()) == null;
        }
    }
}
