using HRTools.Core.Common.Enums;

namespace HRTools.Core.Common.Models.Employee
{
    public class EmployeeFilter
    {
        public string Country { get; set; }
        
        public string City { get; set; }
        
        public EmployeeStatus Status { get; set; }
    }
}
