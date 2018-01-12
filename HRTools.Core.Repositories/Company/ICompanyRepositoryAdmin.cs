using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRTools.Core.Repositories.Company
{
    public interface ICompanyRepositoryAdmin
    {
        Task<IEnumerable<OfficeLocationDto>> GetOfficeLocationListAsync();
    }
}
