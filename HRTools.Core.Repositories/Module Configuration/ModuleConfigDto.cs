using System;

namespace HRTools.Core.Repositories.Module_Configuration
{
    public class ModuleConfigDto
    {
        public int Id { get; set; }
        
        public Guid ClientId { get; set; }

        public string ModuleName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
