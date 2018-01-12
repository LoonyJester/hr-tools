using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRTools.ProjectAssignment.Services.Project
{
    public interface IProjectService
    {
        Task<IEnumerable<Common.Models.Project.Project>> GetProjectsByNameAutocompleteAsync(string nameAutocomplete, bool showDeactivated, bool showOld);
    }
}
