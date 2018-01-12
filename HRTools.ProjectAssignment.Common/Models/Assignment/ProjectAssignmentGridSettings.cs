using HRTools.Crosscutting.Common.Models;

namespace HRTools.ProjectAssignment.Common.Models.Assignment
{
    public class ProjectAssignmentGridSettings : GridSettings
    {
        public string SearchKeyword { get; set; }

        public ProjectAssignmentFilter ProjectAssignmentFilter { get; set; }
    }
}