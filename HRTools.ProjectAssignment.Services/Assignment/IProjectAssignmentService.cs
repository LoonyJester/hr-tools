using System.Threading.Tasks;
using HRTools.Crosscutting.Common.Models;
using HRTools.ProjectAssignment.Common.Models.Assignment;

namespace HRTools.ProjectAssignment.Services.Assignment
{
    public interface IProjectAssignmentService
    {
        Task<GrigData<Common.Models.Assignment.ProjectAssignment>> GetAllAsync(ProjectAssignmentGridSettings settings);

        Task<int> CreateAsync(Common.Models.Assignment.ProjectAssignment projectAssignment);

        Task<int> UpdateAsync(Common.Models.Assignment.ProjectAssignment projectAssignment);

        Task<int> DeleteAsync(int id);
    }
}
