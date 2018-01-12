using System.Threading.Tasks;

namespace HRTools.Crosscutting.Common.Master
{
    public interface IConfigurationManager
    {
        Task CreateConfigurationInCacheAsync();

        Task<ClientConfiguration> GetConfigurationAsync();
    }
}
