using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRTools.Core.Services.Technology
{
    public interface ITechnologyService
    {
        Task<IEnumerable<Common.Models.Technology>> GetAllAsync();
    }
}
