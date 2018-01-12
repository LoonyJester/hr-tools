using HRTools.Crosscutting.Common.Models;

namespace HRTools.ProjectAssignment.Common.Models.Project
{
    public class ProjectGridSettings : GridSettings
    {
        public string SearchKeyword { get; set; }

        public bool ShowOld { get; set; }

        public bool ShowDeactivated { get; set; }
    }
}