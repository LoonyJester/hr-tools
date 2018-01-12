using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRTools.Core.Repositories.JobTitle
{
    public interface IJobTitleRepositoryAdmin
    {
        Task<IEnumerable<JobTitleDto>> GetAllAsync();

        Task<int> CreateAsync(JobTitleDto jobTitleDto);

        Task<int> UpdateAsync(JobTitleDto jobTitleDto);

        Task<JobTitleDto> GetByNameAsync(string name);

        Task<int> DeleteAsync(int id);
    }
}
