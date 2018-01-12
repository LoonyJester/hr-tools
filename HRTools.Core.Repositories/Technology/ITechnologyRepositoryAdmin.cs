using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRTools.Core.Repositories.Technology
{
    public interface ITechnologyRepositoryAdmin
    {
        Task<IEnumerable<TechnologyDto>> GetAllAsync();

        Task<int> CreateAsync(TechnologyDto technologyDto);

        Task<int> UpdateAsync(TechnologyDto technologyDto);

        Task<TechnologyDto> GetByNameAsync(string name);

        Task<int> DeleteAsync(int id);
    }
}
