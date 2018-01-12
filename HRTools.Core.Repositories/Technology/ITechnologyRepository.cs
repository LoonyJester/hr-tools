using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRTools.Core.Repositories.Technology
{
    public interface ITechnologyRepository
    {
        Task<IEnumerable<TechnologyDto>> GetAllAsync();
    }
}
