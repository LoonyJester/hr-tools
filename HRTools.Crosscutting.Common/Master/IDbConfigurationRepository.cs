using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRTools.Crosscutting.Common.Master
{
    public interface IDbConfigurationRepository
    {
        Task<List<ClientConfigurationDto>> GetAllAsync();

        Task<ClientConfigurationDto> GetByClientIdAsync(Guid clientId);

        Task<List<string>> GetActiveModulesByClientIdAsync(Guid clientId);
    }
}
