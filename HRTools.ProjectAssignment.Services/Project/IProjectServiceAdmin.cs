using System.Threading.Tasks;
using HRTools.Crosscutting.Common.Models;
using HRTools.ProjectAssignment.Common.Models.Project;

namespace HRTools.ProjectAssignment.Services.Project
{
    public interface IProjectServiceAdmin
    {
        Task<GrigData<Common.Models.Project.Project>> GetAllAsync(ProjectGridSettings settings);

        Task<int> CreateAsync(Common.Models.Project.Project project);

        Task<int> UpdateAsync(Common.Models.Project.Project project);

        Task<int> DeleteAsync(int id);

        Task<int> ActivateAsync(int id, bool makeActive);
    }
}
