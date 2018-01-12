using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRTools.ProjectAssignment.Services.Department
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Common.Models.Department.Department>> GetAllAsync();
    }
}
