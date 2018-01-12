using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRTools.ProjectAssignment.Services.Department
{
    public interface IDepartmentServiceAdmin
    {
        Task<IEnumerable<Common.Models.Department.Department>> GetAllAsync(string searchKeyword);

        Task<int> CreateAsync(Common.Models.Department.Department department);

        Task<int> UpdateAsync(Common.Models.Department.Department department);

        Task<int> DeleteAsync(int id);
    }
}
