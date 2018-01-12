using System;

namespace HRTools.Crosscutting.Common.Master
{
    public class ClientConfigurationDto
    {
        public Guid ClientId { get; set; }

        public string ConnectionString { get; set; }

        public string ActiveModules { get; set; }
    }
}
