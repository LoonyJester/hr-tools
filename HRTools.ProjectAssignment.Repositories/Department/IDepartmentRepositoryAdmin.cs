using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRTools.ProjectAssignment.Repositories.Department
{
    public interface IDepartmentRepositoryAdmin
    {
        Task<IEnumerable<DepartmentDto>> GetAllAsync(string searchKeyword);

        Task<int> CreateAsync(DepartmentDto departmentDto);

        Task<int> UpdateAsync(DepartmentDto departmentDto);

        Task<DepartmentDto> GetByNameAsync(string name);

        Task<int> DeleteAsync(int id);
    }
}
