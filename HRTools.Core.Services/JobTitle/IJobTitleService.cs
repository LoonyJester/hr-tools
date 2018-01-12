using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRTools.Core.Services.JobTitle
{
    public interface IJobTitleService
    {
        Task<IEnumerable<Common.Models.JobTitle>> GetAllAsync();
    }
}
