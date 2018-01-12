using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.DataAccess;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Crosscutting.Common.Master;

namespace HRTools.Core.Repositories.User
{
    public class UserRepository : Repository, IUserRepositoryAdmin
    {
        public UserRepository(IConnectionFactory connectionFactory,
            IConfigurationManager configurationManager, ILogger logger)
            : base(connectionFactory, configurationManager, logger)
        {
        }

        public Task<int> AssignUserToEmployeeAsync(string userId, string login)
        {
            Guard.ArgumentIsNotNullOrEmpty(userId, nameof(userId));
            Guard.ArgumentIsNotNullOrEmpty(login, nameof(login));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("UserId", userId);
            parameters.Add("Login", login);

            return ExecuteAsync(StoredProcedures.User.AssignUserToEmployee, parameters);
        }
    }
}