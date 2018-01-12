using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.DataAccess;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Crosscutting.Common.Master;
using HRTools.ProjectAssignment.Common.Models.Project;

namespace HRTools.ProjectAssignment.Repositories.Project
{
    public class ProjectRepository : Repository, IProjectRepository, IProjectRepositoryAdmin
    {
        public ProjectRepository(IConnectionFactory connectionFactory,
            IConfigurationManager configurationManager, ILogger logger) :
            base(connectionFactory, configurationManager, logger)
        {
        }

        #region Public

        public Task<IEnumerable<ProjectDto>> GetProjectsByNameAutocomplete(string nameAutocomplete, bool showDeactivated, bool showOld)
        {
            Guard.ArgumentIsNotNullOrEmpty(nameAutocomplete, nameof(nameAutocomplete));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("NameAutocomplete", nameAutocomplete);
            parameters.Add("ShowDeactivated", showDeactivated);
            parameters.Add("ShowOld", showOld);

            return QueryAsync<ProjectDto>(StoredProcedures.Project.GetAllByNameAutocomplete, parameters);
        }

        #endregion

        #region Admin

        public Task<IEnumerable<ProjectDto>> GetAllAsync(ProjectGridSettings settings)
        {
            Guard.ArgumentIsNotNull(settings, nameof(settings));
            Guard.ArgumentIsNotNull(settings.PagingSettings, nameof(settings.PagingSettings));
            Guard.ArgumentIsNotNull(settings.SortingSettings, nameof(settings.SortingSettings));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("CurrentPage", settings.PagingSettings.CurrentPage);
            parameters.Add("ItemsPerPage", settings.PagingSettings.ItemsPerPage);
            parameters.Add("SortColumnName", settings.SortingSettings.SortColumnName);
            parameters.Add("IsDescending", settings.SortingSettings.IsDescending);
            parameters.Add("SearchKeyword", settings.SearchKeyword ?? string.Empty);
            parameters.Add("ShowOld", settings.ShowOld);
            parameters.Add("ShowDeactivated", settings.ShowDeactivated);

            return QueryAsync<ProjectDto>(StoredProcedures.Project.GetAllAdmin, parameters);
        }

        public Task<int> GetTotalCountAsync(ProjectGridSettings settings)
        {
            Guard.ArgumentIsNotNull(settings, nameof(settings));
            Guard.ArgumentIsNotNull(settings.PagingSettings, nameof(settings.PagingSettings));
            Guard.ArgumentIsNotNull(settings.SortingSettings, nameof(settings.SortingSettings));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("SearchKeyword", settings.SearchKeyword ?? string.Empty);
            parameters.Add("ShowOld", settings.ShowOld);
            parameters.Add("ShowDeactivated", settings.ShowDeactivated);

            return QuerySingleOrDefaultAsync<int>(StoredProcedures.Project.GetTotalCount, parameters);
        }

        public Task<int> CreateAsync(ProjectDto projectDto)
        {
            Guard.ArgumentIsNotNull(projectDto, nameof(projectDto));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("Name", projectDto.Name);
            parameters.Add("Description", projectDto.Description);
            parameters.Add("StartDate", projectDto.StartDate);
            parameters.Add("EndDate", projectDto.EndDate);

            return QueryFirstOrDefaultAsync<int>(StoredProcedures.Project.Create, parameters);
        }

        public Task<int> UpdateAsync(ProjectDto projectDto)
        {
            Guard.ArgumentIsNotNull(projectDto, nameof(projectDto));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("Id", projectDto.Id);
            parameters.Add("Name", projectDto.Name);
            parameters.Add("Description", projectDto.Description);
            parameters.Add("StartDate", projectDto.StartDate);
            parameters.Add("EndDate", projectDto.EndDate);

            return ExecuteAsync(StoredProcedures.Project.Update, parameters);
        }

        public Task<ProjectDto> GetByNameAsync(string name)
        {
            Guard.ArgumentIsNotNullOrWhiteSpace(name, nameof(name));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("Name", name);

            return QuerySingleOrDefaultAsync<ProjectDto>(StoredProcedures.Project.GetByName, parameters);
        }

        public Task<int> DeleteAsync(int id)
        {
            Guard.ArgumentIsNotNull(id, nameof(id));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("Id", id);

            return ExecuteAsync(StoredProcedures.Project.Delete, parameters);
        }

        public Task<int> ActivateAsync(int id, bool makeActive)
        {
            Guard.ArgumentIsNotNull(id, nameof(id));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("Id", id);
            parameters.Add("MakeActive", makeActive);

            return ExecuteAsync(StoredProcedures.Project.Activate, parameters);
        }

        #endregion
    }
}