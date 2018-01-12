using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using HRTools.AuthorizationServer.Entities;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.Exceptions;
using HRTools.Crosscutting.Common.Master;
using HRTools.Crosscutting.Common.Models;
using HRTools.Crosscutting.Common.Models.User;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.MySqlClient;
using CoreModels = HRTools.Crosscutting.Common.Models.User;

namespace HRTools.AuthorizationServer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfigurationManager _configurationManager;
        
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private const string UserWithSameEmailExistsMessage = "User with same email already exists.";

        public AuthService(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
            
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new AuthContext(ConnectionString)));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new AuthContext(ConnectionString)));
        }

        private string ConnectionString => GetConnectionString().Result;

        private async Task<string> GetConnectionString()
        {
            ClientConfiguration configuration = await _configurationManager.GetConfigurationAsync();

            return configuration.ConnectionString;
        }
        
        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            return await _userManager.FindAsync(userName, password);
        }

        public Client FindClient(string clientId)
        {
            using (AuthContext context = new AuthContext(ConnectionString))
            {
                var client = context.Clients.Find(clientId);

                return client;
            }
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {
            int res;

            using (AuthContext context = new AuthContext(ConnectionString))
            {
                var existingToken = context.RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();

                if (existingToken != null)
                {
                    var result = await RemoveRefreshToken(existingToken);
                }

                context.RefreshTokens.Add(token);
                
                res = await context.SaveChangesAsync();
            }

            return res > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            using (AuthContext context = new AuthContext(ConnectionString))
            {
                var refreshToken = await context.RefreshTokens.FindAsync(refreshTokenId);

                if (refreshToken != null)
                {
                    context.Entry(refreshToken).State = EntityState.Deleted;

                    return await context.SaveChangesAsync() > 0;
                }

                return false;
            }
        }

        private async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            using (AuthContext context = new AuthContext(ConnectionString))
            {
                context.Entry(refreshToken).State = EntityState.Deleted;

                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            using (AuthContext context = new AuthContext(ConnectionString))
            {
                var refreshToken = await context.RefreshTokens.FindAsync(refreshTokenId);

                return refreshToken;
            }
        }

        Task<GrigData<CoreModels.User>> IAuthService.GetAllAsync(UserTableSettings settings)
        {
            Guard.ArgumentIsNotNull(settings, nameof(settings));
            Guard.ArgumentIsNotNull(settings.PagingSettings, nameof(settings.PagingSettings));
            Guard.ArgumentIsNotNull(settings.UserFilter, nameof(settings.UserFilter));

            IEnumerable<IdentityUser> preselectedData = GetPreselectedData(settings);

            List<CoreModels.User> dtoList = GetFilteredData(preselectedData, settings);
            int totalCount = preselectedData.ToList().Count;

            // TODO: find employee with companyEmail = Login
            //Dictionary<string, string> employees = await _employeeRepositoryAdmin.GetAllCompanyEmailFullNameAsync();

            //foreach (Crosscutting.Common.Models.User.User user in result)
            //{
            //    if (employees.ContainsKey(user.Login))
            //    {
            //        user.AssignedEmployeeName = employees[user.Login];
            //    }
            //}

            return Task.FromResult(new GrigData<CoreModels.User>
            {
                Data = dtoList,
                TotalCount = totalCount
            });
        }
        
        private IEnumerable<IdentityUser> GetPreselectedData(UserTableSettings settings)
        {
            IQueryable<IdentityUser> filteredBySearchKeyword = _userManager.Users.Where(x =>
                x.UserName.Contains(settings.SearchKeyword));
            
            if (settings.UserFilter.IsActivated.HasValue)
            {
                DateTime currentDateTime = DateTime.UtcNow;

                filteredBySearchKeyword =
                    filteredBySearchKeyword.Where(
                        x =>
                            settings.UserFilter.IsActivated.Value
                                ? (x.LockoutEndDateUtc == null ||
                                    DateTime.Compare(x.LockoutEndDateUtc.Value, currentDateTime) < 0)
                                : (x.LockoutEndDateUtc.HasValue &&
                                    DateTime.Compare(x.LockoutEndDateUtc.Value, currentDateTime) >= 0));
            }
            
            List<string> roleIdList = settings.UserFilter.Roles.Select(x => x.Id).ToList();

            IEnumerable<IdentityUser> filteredByRoles = filteredBySearchKeyword.ToList().Where(x =>
            {
                List<string> idList = x.Roles.Select(xx => xx.RoleId).ToList();

                return roleIdList.All(roleId => idList.Contains(roleId));
            });

            filteredByRoles = filteredByRoles.OrderByDescending<IdentityUser, DateTime>(x =>
            {
                if (x.LockoutEndDateUtc != null)
                {
                    return x.LockoutEndDateUtc.Value;
                }

                return DateTime.Now;
            })
                .ThenBy(x => x.UserName);

            return filteredByRoles;
        }

        private List<CoreModels.User> GetFilteredData(IEnumerable<IdentityUser> preselectedUsers, UserTableSettings settings)
        {
            int itemsPerPage = Convert.ToInt32(settings.PagingSettings.ItemsPerPage);
            int currentPage = Convert.ToInt32(settings.PagingSettings.CurrentPage);

            List<IdentityUser> users = preselectedUsers
            .Skip(itemsPerPage * (currentPage - 1))
            .Take(itemsPerPage).ToList();
            
            List<CoreModels.User> res = MapIdentityUserListToUserList(users, settings.UserFilter.IsActivated);

            return res;
        }

        private List<CoreModels.User> MapIdentityUserListToUserList(
            IEnumerable<IdentityUser> identityUsers, bool? userIsLockedFilter)
        {
            return identityUsers.Select(x => MapIdentityUserToUser(x, userIsLockedFilter)).ToList();
        }

        private CoreModels.User MapIdentityUserToUser(
            IdentityUser identityUser, bool? userIsLockedFilter)
        {
            return new CoreModels.User
            {
                UserId = identityUser.Id,
                Login = identityUser.UserName,
                FullName = identityUser.Email,
                Roles = identityUser.Roles.Select(GetRoleById).ToList(),
                AssignedEmployeeName = "",
                IsActivated = userIsLockedFilter ?? !_userManager.IsLockedOut(identityUser.Id)
            };
        }

        private Role GetRoleById(IdentityUserRole identityUserRole)
        {
            IdentityRole role = _roleManager.Roles.FirstOrDefault(xx => xx.Id == identityUserRole.RoleId);

            return new Role
            {
                Id = role.Id,
                Name = role.Name
            };
        }
        
        public async Task<string> CreateUserAsync(CoreModels.User user)
        {
            Guard.ArgumentIsNotNull(user, nameof(user));

            UserValidator.Validate(user);

            ValdiateIfUserWithSameLoginExists(user);

            IdentityUser identityUser = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = user.Login,
                Email = user.FullName
            };

            foreach (Role role in user.Roles)
            {
                identityUser.Roles.Add(new IdentityUserRole
                {
                    UserId = identityUser.Id,
                    RoleId = role.Id
                });
                identityUser.Claims.Add(new IdentityUserClaim
                {
                    ClaimType = ClaimTypes.Role,
                    ClaimValue = role.Name,
                    UserId = identityUser.Id
                });
            }
            identityUser.LockoutEnabled = true;
            identityUser.PasswordHash = _userManager.PasswordHasher.HashPassword(user.Password);

            IdentityResult result = await _userManager.CreateAsync(identityUser, user.Password);

            return result.Succeeded ? identityUser.Id : string.Empty;
        }

        public async Task<IdentityResult> UpdateUserAsync(CoreModels.User user)
        {
            Guard.ArgumentIsNotNull(user, nameof(user));
            bool securityStampNeedToBeChanged = false;

            UserValidator.Validate(user);

            ValdiateIfUserWithSameLoginExists(user);

            IdentityUser identityUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == user.UserId);

            if (identityUser == null)
            {
                return new IdentityResult("IdentityUser is null");
            }

            if (identityUser.UserName != user.Login)
            {
                identityUser.UserName = user.Login;
                securityStampNeedToBeChanged = true;
            }

            identityUser.Email = user.FullName;

            identityUser.Roles.Clear();

            foreach (Role role in user.Roles)
            {
                identityUser.Roles.Add(new IdentityUserRole
                {
                    UserId = user.UserId,
                    RoleId = role.Id
                });
                
                //identityUser.Claims.Add(new IdentityUserClaim
                //{
                //    Id = maxClaimId++,
                //    ClaimType = ClaimTypes.Role,
                //    ClaimValue = role.Name,
                //    UserId = user.UserId
                //});
            }
            
            if (!string.IsNullOrEmpty(user.Password))
            {
                identityUser.PasswordHash = _userManager.PasswordHasher.HashPassword(user.Password);
                securityStampNeedToBeChanged = true;
            }

            IdentityResult result = await _userManager.UpdateAsync(identityUser);

            if (result.Succeeded && securityStampNeedToBeChanged)
            {
                await _userManager.UpdateSecurityStampAsync(identityUser.Id);
            }

            if (result.Succeeded)
            {
                // TODO: refactor. Is needed because UserManager throws an exception "UserId" is required if Id is empty
                await ClearClaimsManuallyAsync(identityUser.Id);

                foreach (Role role in user.Roles)
                {
                    await AddClaimsManuallyAsync(user.UserId, role.Name);
                }
            }

            return result;

        }

        
        private async Task<int> ClearClaimsManuallyAsync(string userId)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(@"delete from `aspnetuserclaims` where UserId = @UserId;", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    int result = await command.ExecuteNonQueryAsync();
                    
                    return result;
                }
            }
        }

        private async Task<int> AddClaimsManuallyAsync(string userId, string claimValue)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand($@"
                    INSERT INTO aspnetuserclaims(UserId, ClaimType, ClaimValue) VALUES(@UserId, @Role, @Value);", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Role", ClaimTypes.Role);
                    command.Parameters.AddWithValue("@Value", claimValue);

                    int result = await command.ExecuteNonQueryAsync();
                    
                    return result;
                }
            }
        }

        private void ValdiateIfUserWithSameLoginExists(CoreModels.User user)
        {
            List<IdentityUser> userWithSameLoginList = _userManager.Users.Where(x => x.UserName == user.FullName).ToList();

            if (userWithSameLoginList.Count > 1)
            {
                throw new ValidationException(UserWithSameEmailExistsMessage);
            }

            IdentityUser userWithSameLogin = userWithSameLoginList.FirstOrDefault();

            if (userWithSameLogin != null && userWithSameLogin.Id != user.UserId)
            {
                throw new ValidationException(UserWithSameEmailExistsMessage);
            }
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            using (RoleManager<IdentityRole> manager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new AuthContext(ConnectionString))))
            {
                List<IdentityRole> roles = await manager.Roles.ToListAsync();

                return roles.Select(x => new Role
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
            }
        }

        public async Task<IdentityResult> ActivateUserAsync(string userId, bool activate)
        {
            IdentityUser identityUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (activate)
            {
                identityUser.LockoutEndDateUtc = null;
            }
            else
            {
                identityUser.LockoutEndDateUtc = DateTime.UtcNow.AddYears(100);
            }

            return await _userManager.UpdateAsync(identityUser);
        }

        public Task<bool> IsUserLockedOutAsync(string userId)
        {
            return _userManager.IsLockedOutAsync(userId);
        }
    }
}