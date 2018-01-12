using System;

namespace HRTools.ProjectAssignment.Repositories.Assignment
{
    public class ProjectAssignmentDto
    {
        public int Id { get; set; }

        public Guid EmployeeId { get; set; }

        public string EmployeeFullName { get; set; }

        public string EmployeeJobTitle { get; set; }

        public string EmployeeTechnology { get; set; }

        public int? ProjectId { get; set; }

        public string ProjectName { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int AssignedForInPersents { get; set; }

        public int BillableForInPersents { get; set; }

        public int AssignedForInPersentsSum { get; set; }

        public int BillableForInPersentsSum { get; set; }
    }
}
