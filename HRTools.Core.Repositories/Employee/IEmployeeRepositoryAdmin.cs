using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRTools.Core.Common.Models.Employee;

namespace HRTools.Core.Repositories.Employee
{
    public interface IEmployeeRepositoryAdmin
    {
        Task<IEnumerable<EmployeeDto>> GetAllAsync(EmployeeGridSettings settings);

        Task<int> GetTotalCountAsync(EmployeeGridSettings settings);

        Task<EmployeeDto> GetByIdAsync(Guid id);

        Task<Guid?> CreateAsync(EmployeeDto dto);

        Task<int> UpdateAsync(EmployeeDto dto);

        Task<EmployeeDto> GetByCompanyEmailAsync(string companyEmail);

        Task<int> DeleteAsync(Guid id);

        Task<int> DeleteBioUrlAsync(Guid id);

        Task<int> DeletePhotoUrlAsync(Guid id);

        Task<Dictionary<string, string>> GetAllCompanyEmailFullNameAsync();
    }
}
