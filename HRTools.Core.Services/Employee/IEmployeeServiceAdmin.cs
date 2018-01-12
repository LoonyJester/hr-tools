using System;
using System.Threading.Tasks;
using HRTools.Core.Common.Models.Employee;
using HRTools.Crosscutting.Common.Models;

namespace HRTools.Core.Services.Employee
{
    public interface IEmployeeServiceAdmin
    {
        Task<GrigData<Common.Models.Employee.Employee>> GetAllAsync(EmployeeGridSettings settings);

        Task<Common.Models.Employee.Employee> GetByIdAsync(Guid id);

        Task<Common.Models.Employee.Employee> GetByCompanyEmailAsync(string companyEmail);

        Task<Guid?> CreateAsync(Common.Models.Employee.Employee employee);

        Task<int> UpdateAsync(Common.Models.Employee.Employee employee);

        Task<int> DeleteAsync(Guid id);

        Task<int> DeleteBioUrlAsync(Guid id);

        Task<int> DeletePhotoUrlAsync(Guid id);
    }
}
