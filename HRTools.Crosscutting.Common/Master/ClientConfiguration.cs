using System;
using System.Collections.Generic;

namespace HRTools.Crosscutting.Common.Master
{
    public class ClientConfiguration
    {
        public Guid ClientId { get; set; }

        public string ConnectionString { get; set; }

        public List<string> ActiveModules { get; set; }
    }
}
