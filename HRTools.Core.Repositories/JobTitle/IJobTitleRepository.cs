using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRTools.Core.Repositories.JobTitle
{
    public interface IJobTitleRepository
    {
        Task<IEnumerable<JobTitleDto>> GetAllAsync();
    }
}
