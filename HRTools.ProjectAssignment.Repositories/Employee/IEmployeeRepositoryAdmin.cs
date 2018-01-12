using System;
using System.Threading.Tasks;

namespace HRTools.ProjectAssignment.Repositories.Employee
{
    public interface IEmployeeRepositoryAdmin
    {
        Task<Guid?> CreateAsync(EmployeeDto employee);

        Task<int> UpdateAsync(EmployeeDto employee);

        Task<int> DeleteAsync(Guid employeeId);
    }
}
