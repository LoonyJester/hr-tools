using System.Threading.Tasks;
using AutoMapper;
using HRTools.Core.Repositories.Employee;
using HRTools.Core.Repositories.User;
using HRTools.Crosscutting.Common;

namespace HRTools.Core.Services.User
{
    public class UserService : IUserServiceAdmin
    {
        private readonly IMapper _mapper;
        private readonly IUserRepositoryAdmin _userRepositoryAdmin;
        private readonly IEmployeeRepositoryAdmin _employeeRepositoryAdmin;

        public UserService(
            IUserRepositoryAdmin userRepositoryAdmin,
            IEmployeeRepositoryAdmin employeeRepositoryAdmin,
            IMapper mapper)
        {
            Guard.ConstructorArgumentIsNotNull(userRepositoryAdmin, nameof(userRepositoryAdmin));
            Guard.ConstructorArgumentIsNotNull(employeeRepositoryAdmin, nameof(employeeRepositoryAdmin));
            Guard.ConstructorArgumentIsNotNull(mapper, nameof(mapper));

            _userRepositoryAdmin = userRepositoryAdmin;
            _employeeRepositoryAdmin = employeeRepositoryAdmin;
            _mapper = mapper;
        }
        
        public async Task<int> AssignUserToEmployeeAsync(string userId, string login)
        {
            Guard.ArgumentIsNotNullOrEmpty(userId, nameof(userId));
            Guard.ArgumentIsNotNullOrEmpty(login, nameof(login));

            return await _userRepositoryAdmin.AssignUserToEmployeeAsync(userId, login);
        }
    }
}