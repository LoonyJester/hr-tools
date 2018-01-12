using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRTools.ProjectAssignment.Repositories.Department
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<DepartmentDto>> GetAllAsync();
    }
}
