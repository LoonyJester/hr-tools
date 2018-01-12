using System.Collections.Generic;
using System.Threading.Tasks;
using HRTools.Core.Common.Models.Employee;

namespace HRTools.Core.Repositories.Employee
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeDto>> GetAllAsync(EmployeeGridSettings settings);

        Task<int> GetTotalCountAsync(EmployeeGridSettings settings);
    }
}
