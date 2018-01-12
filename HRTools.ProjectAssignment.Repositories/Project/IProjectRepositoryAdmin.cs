using System.Collections.Generic;
using System.Threading.Tasks;
using HRTools.ProjectAssignment.Common.Models.Project;

namespace HRTools.ProjectAssignment.Repositories.Project
{
    public interface IProjectRepositoryAdmin
    {
        Task<IEnumerable<ProjectDto>> GetAllAsync(ProjectGridSettings settings);

        Task<int> GetTotalCountAsync(ProjectGridSettings settings);

        Task<int> CreateAsync(ProjectDto projectDto);

        Task<int> UpdateAsync(ProjectDto projectDto);

        Task<ProjectDto> GetByNameAsync(string name);

        Task<int> DeleteAsync(int id);

        Task<int> ActivateAsync(int id, bool makeActive);
    }
}
