using System;

namespace HRTools.Core.Repositories.Employee
{
    public class EmployeeDto
    {
        public Guid EmployeeId { get; set; }

        public string FullName { get; set; }
        
        public string FullNameCyrillic { get; set; }
        
        public string PatronymicCyrillic { get; set; }
        
        public string JobTitle { get; set; }

        public string DepartmentName { get; set; }

        public int? DepartmentId { get; set; }
        
        public string Technology { get; set; }

        public string ProjectName { get; set; }

        public int? ProjectId { get; set; }

        public string CompanyEmail { get; set; }
        
        public string PersonalEmail { get; set; }
        
        public string MessengerName { get; set; }

        public string MessengerLogin { get; set; }
        
        public string MobileNumber { get; set; }
        
        public string AdditionalMobileNumber { get; set; }
        
        public DateTime? Birthday { get; set; }
        
        public int Status { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime? TerminationDate { get; set; }
        
        public int DaysSkipped { get; set; }
        
        public string BioUrl { get; set; }
        
        public string Notes { get; set; }
        
        public string PhotoUrl { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public bool IsDeleted { get; set; }
    }
}
