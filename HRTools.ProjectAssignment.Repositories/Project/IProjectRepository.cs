using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRTools.ProjectAssignment.Repositories.Project
{
    public interface IProjectRepository
    {
        Task<IEnumerable<ProjectDto>> GetProjectsByNameAutocomplete(string nameAutocomplete, bool showDeactivated, bool showOld);
    }
}
