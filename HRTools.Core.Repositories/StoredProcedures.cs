namespace HRTools.Core.Repositories
{
    internal static class StoredProcedures
    {
        internal static class JobTitle
        {
            internal static string GetAll => "core_jobTitle_getAll";
            internal static string Create => "core_jobTitle_create";
            internal static string Update => "core_jobTitle_update";
            internal static string GetByName => "core_jobTitle_getByName";
            internal static string Delete => "core_jobTitle_delete";
        }

        internal static class Technology
        {
            internal static string GetAll => "core_technology_getAll";
            internal static string Create => "core_technology_create";
            internal static string Update => "core_technology_update";
            internal static string GetByName => "core_technology_getByName";
            internal static string Delete => "core_technology_delete";
        }

        internal static class Employee
        {
            internal static string GetAll => "core_employee_getAll";
            internal static string GetById => "core_employee_getById";
            internal static string GetByCompanyEmail => "core_employee_getByCompanyEmail";
            internal static string GetAllAdmin => "core_employee_getAllAdmin";
            internal static string GetTotalCount => "core_employee_getTotalCount";
            internal static string Create => "core_employee_create";
            internal static string Update => "core_employee_update";
            internal static string Delete => "core_employee_delete";
            internal static string DeleteBioUrl => "core_employee_deleteBioUrl";
            internal static string DeletePhotoUrl => "core_employee_deletePhotoUrl";
            internal static string GetAllCompanyEmailFullName => "core_employee_getAllCompanyEmailFullName";
        }

        internal static class Company
        {
            internal static string GetOfficeLocationList => "team_international.core_officelocation_getAll";
        }

        internal static class User
        {
            internal static string AssignUserToEmployee => "core_user_assignUserToEmployee";
        }

        internal static class ModulesConfiguration
        {
            internal static string GetAll => "master_modulesconfiguration_getAll";
            internal static string Create => "master_modulesconfiguration_create";
            internal static string Update => "master_modulesconfiguration_update";
        }
    }
}
