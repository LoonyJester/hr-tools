using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRTools.Core.Services.JobTitle
{
    public interface IJobTitleServiceAdmin
    {
        Task<IEnumerable<Common.Models.JobTitle>> GetAllAsync();

        Task<int> CreateAsync(Common.Models.JobTitle jobTitle);

        Task<int> UpdateAsync(Common.Models.JobTitle jobTitle);

        Task<int> DeleteAsync(int id);
    }
}
