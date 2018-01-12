using System;
using HRTools.ProjectAssignment.Common.Models.Core;

namespace HRTools.ProjectAssignment.Common.Models.Assignment
{
    public class ProjectAssignment
    {
        public int Id { get; set; }

        public Employee Employee { get; set; }

        public Project.Project Project { get; set; }

        public Department.Department Department { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int AssignedForInPersents { get; set; }

        public int BillableForInPersents { get; set; }

        public int AssignedForInPersentsSum { get; set; }

        public int BillableForInPersentsSum { get; set; }
    }
}
