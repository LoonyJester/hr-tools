using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.DataAccess;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Crosscutting.Common.Master;
using HRTools.ProjectAssignment.Common.Models.Assignment;

namespace HRTools.ProjectAssignment.Repositories.Assignment
{
    public class ProjectAssignmentRepository : Repository, IProjectAssignmentRepository
    {
        public ProjectAssignmentRepository(IConnectionFactory connectionFactory,
            IConfigurationManager configurationManager, ILogger logger) :
            base(connectionFactory, configurationManager, logger)
        {
        }

        public Task<IEnumerable<ProjectAssignmentDto>> GetAllAsync(ProjectAssignmentGridSettings settings)
        {
            Guard.ArgumentIsNotNull(settings, nameof(settings));
            Guard.ArgumentIsNotNull(settings.PagingSettings, nameof(settings.PagingSettings));
            Guard.ArgumentIsNotNull(settings.SortingSettings, nameof(settings.SortingSettings));
            Guard.ArgumentIsNotNull(settings.ProjectAssignmentFilter, nameof(settings.ProjectAssignmentFilter));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("CurrentPage", settings.PagingSettings.CurrentPage);
            parameters.Add("ItemsPerPage", settings.PagingSettings.ItemsPerPage);

            parameters.Add("SortColumnName", settings.SortingSettings.SortColumnName);
            parameters.Add("IsDescending", settings.SortingSettings.IsDescending);

            parameters.Add("SearchKeyword", settings.SearchKeyword ?? string.Empty);
            parameters.Add("EmployeeFullNameFilter", settings.ProjectAssignmentFilter.EmployeeFullName ?? string.Empty);
            parameters.Add("EmployeeJobTitleFilter", settings.ProjectAssignmentFilter.EmployeeJobTitle ?? string.Empty);
            parameters.Add("EmployeeTechnologyFilter",
                settings.ProjectAssignmentFilter.EmployeeTechnology ?? string.Empty);
            parameters.Add("ProjectFilter", settings.ProjectAssignmentFilter.ProjectName ?? string.Empty);
            parameters.Add("DepartmentFilter", settings.ProjectAssignmentFilter.DepartmentName ?? string.Empty);
            parameters.Add("ShowOldAssignments", settings.ProjectAssignmentFilter.ShowOldAssignments);
            parameters.Add("ShowOldDeactivatedProjects", settings.ProjectAssignmentFilter.ShowOldDeactivatedProjects);

            return QueryAsync<ProjectAssignmentDto>(StoredProcedures.ProjectAssignment.GetAll, parameters);
        }

        public Task<int> GetTotalCountAsync(ProjectAssignmentGridSettings settings)
        {
            Guard.ArgumentIsNotNull(settings, nameof(settings));
            Guard.ArgumentIsNotNull(settings.PagingSettings, nameof(settings.PagingSettings));
            Guard.ArgumentIsNotNull(settings.SortingSettings, nameof(settings.SortingSettings));
            Guard.ArgumentIsNotNull(settings.ProjectAssignmentFilter, nameof(settings.ProjectAssignmentFilter));

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("SearchKeyword", settings.SearchKeyword ?? string.Empty);
            parameters.Add("EmployeeFullNameFilter", settings.ProjectAssignmentFilter.EmployeeFullName ?? string.Empty);
            parameters.Add("EmployeeJobTitleFilter", settings.ProjectAssignmentFilter.EmployeeJobTitle ?? string.Empty);
            parameters.Add("EmployeeTechnologyFilter",
                settings.ProjectAssignmentFilter.EmployeeTechnology ?? string.Empty);
            parameters.Add("ProjectFilter", settings.ProjectAssignmentFilter.ProjectName ?? string.Empty);
            parameters.Add("DepartmentFilter", settings.ProjectAssignmentFilter.DepartmentName ?? string.Empty);
            parameters.Add("ShowOldAssignments", settings.ProjectAssignmentFilter.ShowOldAssignments);
            parameters.Add("ShowOldDeactivatedProjects", settings.ProjectAssignmentFilter.ShowOldDeactivatedProjects);

            return QuerySingleOrDefaultAsync<int>(StoredProcedures.ProjectAssignment.GetTotalCount, parameters);
        }

        public Task<int> CreateAsync(ProjectAssignmentDto dto)
        {
            Guard.ArgumentIsNotNull(dto, nameof(dto));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("EmployeeId", dto.EmployeeId);
            parameters.Add("ProjectId", dto.ProjectId);
            parameters.Add("DepartmentId", dto.DepartmentId);
            parameters.Add("StartDate", dto.StartDate);
            parameters.Add("EndDate", dto.EndDate);
            parameters.Add("AssignedFor", dto.AssignedForInPersents);
            parameters.Add("BillableFor", dto.BillableForInPersents);

            return QueryFirstOrDefaultAsync<int>(StoredProcedures.ProjectAssignment.Create, parameters);
        }

        public Task<int> UpdateAsync(ProjectAssignmentDto dto)
        {
            Guard.ArgumentIsNotNull(dto, nameof(dto));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("Id", dto.Id);
            parameters.Add("EmployeeId", dto.EmployeeId);
            parameters.Add("ProjectId", dto.ProjectId);
            parameters.Add("DepartmentId", dto.DepartmentId);
            parameters.Add("StartDate", dto.StartDate);
            parameters.Add("EndDate", dto.EndDate);
            parameters.Add("AssignedFor", dto.AssignedForInPersents);
            parameters.Add("BillableFor", dto.BillableForInPersents);

            return ExecuteAsync(StoredProcedures.ProjectAssignment.Update, parameters);
        }

        public Task<int> DeleteAsync(int id)
        {
            Guard.ArgumentIsNotNull(id, nameof(id));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("Id", id);

            return ExecuteAsync(StoredProcedures.ProjectAssignment.Delete, parameters);
        }
    }
}