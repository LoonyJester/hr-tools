using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRTools.Core.Repositories.Company
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<OfficeLocationDto>> GetOfficeLocationListAsync();
    }
}
