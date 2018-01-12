using System.Collections.Generic;
using System.Threading.Tasks;
using HRTools.ProjectAssignment.Common.Models.Assignment;

namespace HRTools.ProjectAssignment.Repositories.Assignment
{
    public interface IProjectAssignmentRepository
    {
        Task<IEnumerable<ProjectAssignmentDto>> GetAllAsync(ProjectAssignmentGridSettings settings);

        Task<int> GetTotalCountAsync(ProjectAssignmentGridSettings settings);

        Task<int> CreateAsync(ProjectAssignmentDto dto);

        Task<int> UpdateAsync(ProjectAssignmentDto dto);

        Task<int> DeleteAsync(int id);
    }
}
