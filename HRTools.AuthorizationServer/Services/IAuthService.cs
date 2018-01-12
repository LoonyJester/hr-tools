using System.Collections.Generic;
using System.Threading.Tasks;
using HRTools.AuthorizationServer.Entities;
using HRTools.Crosscutting.Common.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using CoreUser = HRTools.Crosscutting.Common.Models.User;

namespace HRTools.AuthorizationServer.Services
{
    public interface IAuthService
    {
        Client FindClient(string clientId);

        Task<IdentityUser> FindUser(string userName, string password);

        Task<RefreshToken> FindRefreshToken(string refreshTokenId);

        Task<bool> AddRefreshToken(RefreshToken token);

        Task<bool> RemoveRefreshToken(string refreshTokenId);

        Task<GrigData<CoreUser.User>> GetAllAsync(CoreUser.UserTableSettings settings);

        Task<string> CreateUserAsync(CoreUser.User user);

        Task<IdentityResult> UpdateUserAsync(CoreUser.User user);
        
        Task<List<CoreUser.Role>> GetAllRolesAsync();

        Task<IdentityResult> ActivateUserAsync(string userId, bool activate);

        Task<bool> IsUserLockedOutAsync(string userId);
    }
}
