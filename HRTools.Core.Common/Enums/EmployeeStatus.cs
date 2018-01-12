using System.ComponentModel;

namespace HRTools.Core.Common.Enums
{
    public enum EmployeeStatus
    {
        NotDefined = -1,

        [Description("Hired")]
        Hired = 0,

        [Description("Dismissed")]
        Dismissed = 1
    }
}
