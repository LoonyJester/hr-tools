namespace HRTools.ProjectAssignment.Common.Models.Assignment
{
    public class ProjectAssignmentFilter
    {
        public string EmployeeFullName { get; set; }

        public string EmployeeJobTitle { get; set; }

        public string EmployeeTechnology { get; set; }

        public string ProjectName { get; set; }

        public string DepartmentName { get; set; }

        public bool ShowOldAssignments { get; set; }

        public bool ShowOldDeactivatedProjects { get; set; }
    }
}
