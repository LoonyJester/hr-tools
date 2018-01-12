using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRTools.Core.Services.Technology
{
    public interface ITechnologyServiceAdmin
    {
        Task<IEnumerable<Common.Models.Technology>> GetAllAsync();

        Task<int> CreateAsync(Common.Models.Technology technology);

        Task<int> UpdateAsync(Common.Models.Technology technology);

        Task<int> DeleteAsync(int id);
    }
}
