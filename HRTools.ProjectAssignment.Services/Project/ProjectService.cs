using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Exceptions;
using HRTools.Crosscutting.Common.Models;
using HRTools.ProjectAssignment.Common.Models.Project;
using HRTools.ProjectAssignment.Repositories.Project;

namespace HRTools.ProjectAssignment.Services.Project
{
    public class ProjectService : IProjectService, IProjectServiceAdmin
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectRepositoryAdmin _projectRepositoryAdmin;
        private readonly IMapper _mapper;

        public ProjectService(
            IProjectRepository projectRepository,
            IProjectRepositoryAdmin projectRepositoryAdmin,
            IMapper mapper)
        {
            Guard.ConstructorArgumentIsNotNull(projectRepository, nameof(projectRepository));
            Guard.ConstructorArgumentIsNotNull(projectRepositoryAdmin, nameof(projectRepositoryAdmin));
            Guard.ConstructorArgumentIsNotNull(mapper, nameof(mapper));

            _projectRepository = projectRepository;
            _projectRepositoryAdmin = projectRepositoryAdmin;
            _mapper = mapper;
        }

        #region Public

        public async Task<IEnumerable<Common.Models.Project.Project>> GetProjectsByNameAutocompleteAsync(string nameAutocomplete, bool showDeactivated, bool showOld)
        {
            Guard.ArgumentIsNotNullOrEmpty(nameAutocomplete, nameof(nameAutocomplete));
            
            IEnumerable<ProjectDto> list = await _projectRepository.GetProjectsByNameAutocomplete(nameAutocomplete, showDeactivated, showOld);

            return _mapper.Map<IEnumerable<ProjectDto>, IEnumerable<Common.Models.Project.Project>>(list);
        }

        #endregion

        #region Admin

        public async Task<GrigData<Common.Models.Project.Project>> GetAllAsync(ProjectGridSettings settings)
        {
            Guard.ArgumentIsNotNull(settings, nameof(settings));

            IEnumerable<ProjectDto> dtoList = await _projectRepositoryAdmin.GetAllAsync(settings);
            int totalCount = await _projectRepositoryAdmin.GetTotalCountAsync(settings);

            IEnumerable<Common.Models.Project.Project> result = _mapper.Map<IEnumerable<ProjectDto>, IEnumerable<Common.Models.Project.Project>>(dtoList);

            return new GrigData<Common.Models.Project.Project>
            {
                Data = result,
                TotalCount = totalCount
            };
        }

        public async Task<int> CreateAsync(Common.Models.Project.Project project)
        {
            Guard.ArgumentIsNotNull(project, nameof(project));

            await ValdiateIfProjectWithSameNameExists(project);

            ProjectDto dto = _mapper.Map<Common.Models.Project.Project, ProjectDto>(project);

            return await _projectRepositoryAdmin.CreateAsync(dto);
        }

        public async Task<int> UpdateAsync(Common.Models.Project.Project project)
        {
            Guard.ArgumentIsNotNull(project, nameof(project));

            await ValdiateIfProjectWithSameNameExists(project);

            ProjectDto dto = _mapper.Map<Common.Models.Project.Project, ProjectDto>(project);

            return await _projectRepositoryAdmin.UpdateAsync(dto);
        }

        private async Task ValdiateIfProjectWithSameNameExists(Common.Models.Project.Project project)
        {
            ProjectDto projectWithSameName = await _projectRepositoryAdmin.GetByNameAsync(project.Name);

            if (projectWithSameName != null && projectWithSameName.Id != project.Id)
            {
                throw new ValidationException("Project with same Name already exists.");
            }
        }

        public Task<int> DeleteAsync(int id)
        {
            Guard.ArgumentIsNotNull(id, nameof(id));

            return _projectRepositoryAdmin.DeleteAsync(id);
        }

        public Task<int> ActivateAsync(int id, bool makeActive)
        {
            return _projectRepositoryAdmin.ActivateAsync(id, makeActive);
        }

        #endregion
    }
}