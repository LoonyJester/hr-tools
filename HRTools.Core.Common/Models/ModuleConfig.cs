using System;

namespace HRTools.Core.Common.Models
{
    public class ModuleConfig
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public Guid ClientId { get; set; }
        
        public string ModuleName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
