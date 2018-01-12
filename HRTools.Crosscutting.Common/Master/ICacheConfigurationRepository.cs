using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRTools.Crosscutting.Common.Master
{
    public interface ICacheConfigurationRepository
    {
        ClientConfigurationDto GetByClientId(Guid clientId);

        void CreateConfigurationList(List<ClientConfigurationDto> configurations);

        Task<bool> CreateConfiguration(ClientConfigurationDto configuration);

        bool DeleteConfiguration(Guid clientId);
    }
}
