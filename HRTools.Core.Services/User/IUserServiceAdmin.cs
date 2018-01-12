using System.Threading.Tasks;

namespace HRTools.Core.Services.User
{
    public interface IUserServiceAdmin
    {
        Task<int> AssignUserToEmployeeAsync(string userId, string login);
    }
}
