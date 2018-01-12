using System;

namespace HRTools.Core.Common.Models.Employee
{
    public class Employee
    {
        public Guid EmployeeId { get; set; }

        public string FullName { get; set; }
        
        public string FullNameCyrillic { get; set; }
        
        public string PatronymicCyrillic { get; set; }
        
        public OfficeLocation OfficeLocation { get; set; }

        public string JobTitle { get; set; }

        public string DepartmentName { get; set; }

        //public Department Department { get; set; }
        
        public string Technology { get; set; }
        
        public string ProjectName { get; set; }

        //public Project Project { get; set; }

        public string CompanyEmail { get; set; }
        
        public string PersonalEmail { get; set; }
        
        public Messenger Messenger { get; set; }
        
        public string MobileNumber { get; set; }
        
        public string AdditionalMobileNumber { get; set; }
        
        public DateTime? Birthday { get; set; }
        
        public int Status { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime? TerminationDate { get; set; }
        
        public int DaysSkipped { get; set; }
        
        // todo
        //public string TimeInCompany { get; set; }
        
        public string BioUrl { get; set; }
        
        public string Notes { get; set; }
        
        public string PhotoUrl { get; set; }

        public string EmployeeIdForFiles { get; set; }
    }
}