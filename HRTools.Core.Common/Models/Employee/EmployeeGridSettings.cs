using HRTools.Crosscutting.Common.Models;

namespace HRTools.Core.Common.Models.Employee
{
    public class EmployeeGridSettings: GridSettings
    {
        public string SearchKeyword { get; set; }

        public EmployeeFilter EmployeeFilter { get; set; }
    }
}