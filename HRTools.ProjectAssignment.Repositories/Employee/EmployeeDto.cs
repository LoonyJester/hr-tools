using System;

namespace HRTools.ProjectAssignment.Repositories.Employee
{
    public class EmployeeDto
    {
        public Guid EmployeeId { get; set; }

        public string FullName { get; set; }

        public string JobTitle { get; set; }

        public string Technology { get; set; }

        public DateTime StartDate { get; set; }

        public int AssignedForInPersentsSum { get; set; }

        public int BillableForInPersentsSum { get; set; }
    }
}
