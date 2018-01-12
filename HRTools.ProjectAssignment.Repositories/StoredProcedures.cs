namespace HRTools.ProjectAssignment.Repositories
{
    internal static class StoredProcedures
    {
        internal static class Employee
        {
            internal static string GetAllByNameAutocomplete => "pa_employee_getAllByNameAutocomplete";
            internal static string Create => "pa_employee_create";
            internal static string Update => "pa_employee_update";
            internal static string Delete => "pa_employee_delete";
        }

        internal static class Project
        {
            internal static string GetAllAdmin => "pa_project_getAll";
            internal static string GetTotalCount => "pa_project_getTotalCount";
            internal static string Create => "pa_project_create";
            internal static string Update => "pa_project_update";
            internal static string GetByName => "pa_project_getByName";
            internal static string Delete => "pa_project_delete";
            internal static string Activate => "pa_project_activate";

            internal static string GetAllByNameAutocomplete => "pa_project_getAllByNameAutocomplete";
        }

        internal static class Department
        {
            internal static string GetAll => "pa_department_getAll";
            internal static string GetAllAdmin => "pa_department_getAllAdmin";
            internal static string Create => "pa_department_create";
            internal static string Update => "pa_department_update";
            internal static string GetByName => "pa_department_getByName";
            internal static string Delete => "pa_department_delete";
        }

        internal static class ProjectAssignment
        {
            internal static string GetAll => "pa_projectAssignment_getAll";
            internal static string GetTotalCount => "pa_projectAssignment_getTotalCount";
            internal static string Create => "pa_projectAssignment_create";
            internal static string Update => "pa_projectAssignment_update";
            internal static string Delete => "pa_projectAssignment_delete";
        }
    }
}
