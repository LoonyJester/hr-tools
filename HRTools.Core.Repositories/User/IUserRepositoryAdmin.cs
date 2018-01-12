using System.Threading.Tasks;

namespace HRTools.Core.Repositories.User
{
    public interface IUserRepositoryAdmin
    {
        Task<int> AssignUserToEmployeeAsync(string userId, string login);
    }
}
