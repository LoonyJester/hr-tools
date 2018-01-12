using System.Collections.Generic;
using System.Threading.Tasks;
using HRTools.Core.Common.Models;

namespace HRTools.Core.Services.Company
{
    public interface ICompanyService
    {
        Task<IEnumerable<OfficeLocation>> GetOfficeLocationListAsync();
    }
}
