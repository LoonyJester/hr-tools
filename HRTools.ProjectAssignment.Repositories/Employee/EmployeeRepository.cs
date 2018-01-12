using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.DataAccess;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Crosscutting.Common.Master;

namespace HRTools.ProjectAssignment.Repositories.Employee
{
    public class EmployeeRepository : Repository, IEmployeeRepository, IEmployeeRepositoryAdmin
    {
        public EmployeeRepository(IConnectionFactory connectionFactory, IConfigurationManager configurationManager,
            ILogger logger)
            : base(connectionFactory, configurationManager, logger)
        {
        }

        #region Public 

        public Task<IEnumerable<EmployeeDto>> GetAllByNameAutocompleteAsync(string nameAutocomplete)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("NameAutocomplete", nameAutocomplete);

            return QueryAsync<EmployeeDto>(StoredProcedures.Employee.GetAllByNameAutocomplete, parameters);
        }

        #endregion

        #region Admin

        public async Task<Guid?> CreateAsync(EmployeeDto employee)
        {
            Guard.ArgumentIsNotNull(employee, nameof(employee));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("EmployeeId", employee.EmployeeId);
            parameters.Add("FullName", employee.FullName);
            parameters.Add("JobTitle", employee.JobTitle);
            parameters.Add("Technology", employee.Technology);
            parameters.Add("StartDate", employee.StartDate);

            return await ExecuteAsync(StoredProcedures.Employee.Create, parameters) != 0
                ? (Guid?) employee.EmployeeId
                : null;
        }

        public async Task<int> UpdateAsync(EmployeeDto employee)
        {
            Guard.ArgumentIsNotNull(employee, nameof(employee));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("EmployeeId", employee.EmployeeId);
            parameters.Add("FullName", employee.FullName);
            parameters.Add("JobTitle", employee.JobTitle);
            parameters.Add("Technology", employee.Technology);
            parameters.Add("StartDate", employee.StartDate);

            return await ExecuteAsync(StoredProcedures.Employee.Update, parameters);
        }

        public async Task<int> DeleteAsync(Guid employeeId)
        {
            Guard.ArgumentIsNotNull(employeeId, nameof(employeeId));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("EmployeeId", employeeId);

            return await ExecuteAsync(StoredProcedures.Employee.Delete, parameters);
        }

        #endregion
    }
}