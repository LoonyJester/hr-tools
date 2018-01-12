using System.Text.RegularExpressions;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Exceptions;

namespace HRTools.Core.Services.Employee
{
    public static class EmployeeValidator
    {
        private const int FieldLength = 250;
        private const int EmailLength = 150;
        private const int MobileNumberLength = 20;
        private const int BioUrlLength = 1000;
        private const int NotesLength = 500;
        private const string EmailPattern = @"^(([^<>()\[\]\.,;:\s@\']+(\.[^<>()\[\]\.,;:\s@\']+)*)|(\'.+\'))@(([^<>()[\]\.,;:\s@\']+\.)+[^<>()[\]\.,;:\s@\']{2,})$";

        public static void Validate(Common.Models.Employee.Employee employee)
        {
            Guard.ArgumentIsNotNull(employee, nameof(employee));

            if (string.IsNullOrWhiteSpace(employee.FullName))
            {
                throw new ValidationException("FullName is required");
            }

            if (employee.FullName.Length > FieldLength)
            {
                throw new ValidationException($"FullName length can not be more than {FieldLength} symbols");
            }

            if (employee.FullNameCyrillic != null && employee.FullNameCyrillic.Length > FieldLength)
            {
                throw new ValidationException($"FullNameCyrillic length can not be more than {FieldLength} symbols");
            }

            if (employee.PatronymicCyrillic != null && employee.PatronymicCyrillic.Length > FieldLength)
            {
                throw new ValidationException($"PatronymicCyrillic length can not be more than {FieldLength} symbols");
            }

            if (string.IsNullOrWhiteSpace(employee.JobTitle))
            {
                throw new ValidationException("JobTitle is required");
            }

            if (employee.JobTitle.Length > FieldLength)
            {
                throw new ValidationException($"JobTitle length can not be more than {FieldLength} symbols");
            }

            if (employee.DepartmentName != null && employee.DepartmentName.Length > FieldLength)
            {
                throw new ValidationException($"DepartmentName length can not be more than {FieldLength} symbols");
            }

            if (employee.Technology != null && employee.Technology.Length > FieldLength)
            {
                throw new ValidationException($"Technology length can not be more than {FieldLength} symbols");
            }

            if (employee.ProjectName != null && employee.ProjectName.Length > FieldLength)
            {
                throw new ValidationException($"ProjectName length can not be more than {FieldLength} symbols");
            }

            if (string.IsNullOrWhiteSpace(employee.CompanyEmail))
            {
                throw new ValidationException("CompanyEmail is required");
            }

            if (employee.CompanyEmail.Length > EmailLength)
            {
                throw new ValidationException($"CompanyEmail length can not be more than {EmailLength} symbols");
            }

            if (!Regex.IsMatch(employee.CompanyEmail, EmailPattern))
            {
                throw new ValidationException("CompanyEmail does not have an email format");
            }

            if (employee.PersonalEmail != null && employee.PersonalEmail.Length > EmailLength)
            {
                throw new ValidationException($"PersonalEmail length can not be more than {EmailLength} symbols");
            }

            if (employee.PersonalEmail != null && !Regex.IsMatch(employee.PersonalEmail, EmailPattern))
            {
                throw new ValidationException("PersonalEmail does not have an email format");
            }

            if (employee.Messenger?.Name != null && employee.Messenger.Name.Length > EmailLength)
            {
                throw new ValidationException($"Messanger name length can not be more than {EmailLength} symbols");
            }

            if (employee.Messenger?.Login != null && employee.Messenger.Login.Length > EmailLength)
            {
                throw new ValidationException($"Messanger login length can not be more than {EmailLength} symbols");
            }

            if (employee.MobileNumber != null && employee.MobileNumber.Length > MobileNumberLength)
            {
                throw new ValidationException($"MobileNumber length can not be more than {MobileNumberLength} symbols");
            }

            if (employee.AdditionalMobileNumber != null && employee.AdditionalMobileNumber.Length > MobileNumberLength)
            {
                throw new ValidationException($"AdditionalMobileNumber length can not be more than {MobileNumberLength} symbols");
            }

            if (employee.DaysSkipped < 0)
            {
                throw new ValidationException("DaysSkipped can not be less than zero.");
            }

            if (employee.BioUrl != null && employee.BioUrl.Length > BioUrlLength)
            {
                throw new ValidationException($"AdditionalMobileNumber length can not be more than {BioUrlLength} symbols");
            }

            if (employee.Notes != null && employee.Notes.Length > NotesLength)
            {
                throw new ValidationException($"Notes length can not be more than {NotesLength} symbols");
            }

            if (employee.PhotoUrl != null && employee.PhotoUrl.Length > BioUrlLength)
            {
                throw new ValidationException($"PhotoUrl length can not be more than {BioUrlLength} symbols");
            }
        }
    }
}