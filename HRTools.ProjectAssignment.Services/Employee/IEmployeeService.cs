using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRTools.ProjectAssignment.Services.Employee
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Common.Models.Core.Employee>> GetAllByNameAutocompleteAsync(string nameAutocomplete);
    }
}
