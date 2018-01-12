using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRTools.ProjectAssignment.Repositories.Employee
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeDto>> GetAllByNameAutocompleteAsync(string nameAutocomplete);
    }
}
