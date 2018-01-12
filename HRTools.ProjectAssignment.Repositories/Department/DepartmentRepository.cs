using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using HRTools.Crosscutting.Common;
using HRTools.Crosscutting.Common.DataAccess;
using HRTools.Crosscutting.Common.Logging;
using HRTools.Crosscutting.Common.Master;

namespace HRTools.ProjectAssignment.Repositories.Department
{
    public class DepartmentRepository : Repository, IDepartmentRepository, IDepartmentRepositoryAdmin
    {
        public DepartmentRepository(IConnectionFactory connectionFactory,
            IConfigurationManager configurationManager, ILogger logger) :
            base(connectionFactory, configurationManager, logger)
        {
        }

        #region Public

        public Task<IEnumerable<DepartmentDto>> GetAllAsync()
        {
            DynamicParameters parameters = new DynamicParameters();

            return QueryAsync<DepartmentDto>(StoredProcedures.Department.GetAll, parameters);
        }

        #endregion

        #region Admin

        public Task<IEnumerable<DepartmentDto>> GetAllAsync(string searchKeyword)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("SearchKeyword", searchKeyword ?? string.Empty);

            return QueryAsync<DepartmentDto>(StoredProcedures.Department.GetAllAdmin, parameters);
        }

        public Task<int> CreateAsync(DepartmentDto departmentDto)
        {
            Guard.ArgumentIsNotNull(departmentDto, nameof(departmentDto));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("Name", departmentDto.Name);
            parameters.Add("Description", departmentDto.Description);

            return QueryFirstOrDefaultAsync<int>(StoredProcedures.Department.Create, parameters);
        }

        public Task<int> UpdateAsync(DepartmentDto departmentDto)
        {
            Guard.ArgumentIsNotNull(departmentDto, nameof(departmentDto));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("Id", departmentDto.Id);
            parameters.Add("Name", departmentDto.Name);
            parameters.Add("Description", departmentDto.Description);

            return ExecuteAsync(StoredProcedures.Department.Update, parameters);
        }

        public Task<DepartmentDto> GetByNameAsync(string name)
        {
            Guard.ArgumentIsNotNullOrWhiteSpace(name, nameof(name));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("Name", name);

            return QuerySingleOrDefaultAsync<DepartmentDto>(StoredProcedures.Department.GetByName, parameters);
        }

        public Task<int> DeleteAsync(int id)
        {
            Guard.ArgumentIsNotNull(id, nameof(id));

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("Id", id);

            return ExecuteAsync(StoredProcedures.Department.Delete, parameters);
        }

        #endregion
    }
}