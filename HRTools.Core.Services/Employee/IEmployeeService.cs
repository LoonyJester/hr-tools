using System.Threading.Tasks;
using HRTools.Core.Common.Models.Employee;
using HRTools.Crosscutting.Common.Models;

namespace HRTools.Core.Services.Employee
{
    public interface IEmployeeService
    {
        Task<GrigData<Common.Models.Employee.Employee>> GetAllAsync(EmployeeGridSettings settings);
    }
}
